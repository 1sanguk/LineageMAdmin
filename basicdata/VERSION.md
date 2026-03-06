# LineageMOps — 버전 히스토리

---

## v3.1.0 (2026-03-06)
### 직업 시스템 실제 리니지M 13직업으로 전면 개편

**CharacterClass 열거형 변경 (7종 → 13종)**
- 기존: Knight(군주), DarkKnight(기사), Elf(요정), Wizard(마법사), DarkElf(다크엘프), DragonKnight(드래곤나이트), Illusionist(환술사)
- 변경: Lord(군주), Knight(기사), Elf(요정), Wizard(마법사), DarkElf(다크엘프), Gunner(총사), Fighter(투사), DarkKnight(암흑기사), HolyKnight(신성검사), Berserker(광전사), Reaper(사신), ThunderGod(뇌신), SpellBlade(마검사)

**MockDataStore GenerateStats 스탯 보너스 갱신**
- 직업별 특성 반영: 군주(CHA+4), 기사(STR+4, CON+3), 요정(DEX+3, WIS+3), 마법사(INT+4, WIS+2), 다크엘프(DEX+3, STR+2), 총사(DEX+4, STR+2), 투사(STR+4, CON+2), 암흑기사(STR+2, DTH+2), 신성검사(STR+2, WIS+3, LFE+2), 광전사(STR+4), 사신(DEX+3, DTH+2), 뇌신(INT+3, WIS+2), 마검사(STR+2, INT+3)

**뷰 switch 표현식 13직업 반영**
- `Views/GameData/Detail.cshtml` — className / classColor switch 갱신, `isKnight` → `isLord` (CHA 스탯 설명 조건)
- `Views/GameData/Index.cshtml` — 직업 필터 드롭다운 / 테이블 className switch 갱신
- `Views/User/Detail.cshtml` — 캐릭터 목록 className switch 갱신

**직업별 색상 지정**
- Lord(금), Knight(인디고), Elf(초록), Wizard(파랑), DarkElf(보라), Gunner(오렌지), Fighter(빨강), DarkKnight(진보라), HolyKnight(앰버), Berserker(진홍), Reaper(회색), ThunderGod(시안), SpellBlade(바이올렛)

---

## v3.0.0 (2026-03-06)
### Clean Code 전면 리팩토링 (39개 파일 변경, 11개 신규 생성)

#### 🔴 P1 — 즉시 수정 (명확한 위반)

**죽은 코드 제거**
- `UserController.Index` — 아무도 읽지 않는 `ViewBag.Paged = PaginatedList.Create(...)` 2줄 제거

**Magic Literal 상수화**
- `Constants/AppConstants.cs` 신규 — `MockOperatorId = "op_001"` 상수 정의
- 기존 4개 파일(`UserService`, `SqlUserService`, `AdminLogService`, `SqlAdminLogService`, `EventController`, `AdminLog` 모델)에 흩어진 하드코딩 `"op_001"` 전부 `AppConstants.MockOperatorId` 참조로 교체

**서버 목록 단일화**
- `Constants/GameConstants.cs` 신규 — `ServerNames` 배열 중앙 관리
- `MockDataStore`의 `private static readonly string[] ServerNames`와 `EventController`의 `private static readonly string[] AllServers` 이중 선언 제거
- `MockDataStore`, `EventController`, `MonitoringViewModel`, `Monitoring/Index.cshtml` 등 전 파일이 `GameConstants.ServerNames` 참조

**대시보드 하드코딩 수치 제거**
- `DashboardController` — `TodayNewAccounts = 23`, `TodayNewSanctions = 4` 가짜 상수 제거
- `IUserService.GetTodayNewAccountCount()` 메서드 추가 — 오늘 가입 계정 수 실제 집계
- `IAdminLogService.GetTodayNewSanctionCount()` 메서드 추가 — 오늘 제재 처리 건수 실제 집계
- Mock/SQL 구현체 모두 반영

**PaginatedList 생성 중복 제거**
- `PaginatedList<T>.From(items, totalCount, pageIndex, pageSize)` 정적 팩토리 메서드 추가
- 기존 `Create()` 내부도 `From()` 으로 위임
- 컨트롤러 3곳에서 반복되던 수동 객체 생성 코드 완전 제거

**Enum 네이밍 수정**
- `CharacterClass.Dragonknight` → `CharacterClass.DragonKnight` (Pascal case 일관성)
- 관련 뷰 3개(`GameData/Detail.cshtml`, `GameData/Index.cshtml`, `User/Detail.cshtml`) 일괄 수정

---

#### 🟡 P2 — 설계 개선

**`out` 파라미터 제거**
- `IUserService.Search`, `IGameDataService.SearchCharacters`, `IMonitoringService.GetLogs` 시그니처 변경
- `out int totalCount` 제거 → 서비스가 `PaginatedList<T>` 직접 반환
- 컨트롤러에서 `PaginatedList` 수동 구성 코드 완전 제거
- Mock 서비스 3개 + SQL 서비스 3개 + 인터페이스 3개 모두 갱신

**서비스 계층에서 ViewModel 반환 제거**
- `IUserService.GetDetail(int id) → UserDetailViewModel` 제거 (서비스가 뷰 계층에 의존하는 구조 위반)
- `UserController.Detail` — 직접 `GetById` + `GetCharactersByAccountId` 호출 후 컨트롤러에서 `UserDetailViewModel` 조립
- `UserController` 에 `IGameDataService` 의존성 추가

**캐릭터 조회 메서드 추가**
- `IGameDataService.GetCharactersByAccountId(int accountId)` 신규 — 계정별 캐릭터 목록 조회
- Mock/SQL 구현체 모두 반영

**스탯 diff 로직 서비스로 이동**
- `GameDataController.UpdateStats` 내 13개 스탯 비교 블록 제거
- `IGameDataService.BuildStatChanges(CharacterStats old, CharacterStats updated) → List<string>` 신규
- 컨트롤러는 결과만 받아 어드민 로그 기록 — 단일 책임 원칙 준수
- Mock/SQL 구현체 모두 반영

**필드명 명확화**
- `GameDataController._users` → `_userService` (서비스임을 명시)

**클래스 파일 분리 (한 파일 한 클래스)**
- `Character.cs` (기존 5개 정의) → 5개 파일로 분리
  - `Character.cs` — Character 클래스만
  - `CharacterClass.cs` — CharacterClass enum
  - `CharacterStats.cs` — CharacterStats 클래스
  - `InventoryItem.cs` — InventoryItem 클래스
  - `ItemGrade.cs` — ItemGrade enum
- `Account.cs` (기존 4개 정의) → 4개 파일로 분리
  - `Account.cs` — Account 클래스만
  - `AccountStatus.cs` — AccountStatus enum
  - `BannedRecord.cs` — BannedRecord 클래스
  - `SanctionType.cs` — SanctionType enum

**MockDataStore Singleton 패턴 제거**
- `private static readonly MockDataStore _instance = new()` 제거
- `public static MockDataStore Instance` 제거
- 생성자 `private` → `public` 변경
- `Program.cs` — `AddSingleton(MockDataStore.Instance)` → `AddSingleton<MockDataStore>()` (DI 단일 관리)
- `DbInitializer.cs` — `MockDataStore.Instance` → DI `GetRequiredService<MockDataStore>()`

---

#### 🟢 P3 — 구조 개선

**ViewBag 과다 사용 → 전용 ViewModel**
- `Models/ViewModels/CharacterDetailViewModel.cs` 신규
  - `Character`, `Account?`, `List<Item> AllItems` 포함
  - `GameDataController.Detail` — `ViewBag.Account` / `ViewBag.AllItems` 제거
  - `GameData/Detail.cshtml` — `@model Character` → `@model CharacterDetailViewModel`, 모든 `Model.*` → `Model.Character.*` 갱신
- `Models/ViewModels/MonitoringViewModel.cs` 신규
  - `PaginatedList<ServerLog> Logs`, `List<ServerStatus> Servers`, `FilterType`, `FilterServer`, `FilterSearch`, `AllServerNames` 포함
  - `MonitoringController` — ViewBag 5개 제거, ViewModel 반환
  - `Monitoring/Index.cshtml` — `@model PaginatedList<ServerLog>` → `@model MonitoringViewModel`, 전체 참조 갱신
  - 뷰 내 하드코딩 서버 배열 → `Model.AllServerNames` (GameConstants 연동)

**불필요한 의존성 제거**
- `MonitoringController` — `IAdminLogService` 의존성 제거 (뷰에서 실제로 사용되지 않았음)

**MockDataStore 메서드 분리 (함수 크기 축소)**
- `GenerateInventory` (51줄) → `CreateWeaponItem` / `CreateArmorItem` / `CreateConsumableItems` 3개 추출
- `SeedAccounts` — 제재 생성 로직을 `AddSanctionToAccount` 별도 메서드로 추출
- `SeedLogs` — 로그 항목 생성 로직을 `CreateLogEntry` 별도 메서드로 추출
- `SeedCharacters` — 레벨 범위를 `LevelTable.MaxLevel` 기반으로 수정 (기존 `rand.Next(1, 120)`이 최대 레벨 90을 초과하는 버그 수정)

---

## v2.5.0 (2026-03-05)
### 레벨 / 경험치 관리
- `Models/Domain/LevelTable.cs` 신규 — 레벨 1~90 누적 경험치 임계값 테이블 (실제 리니지M 수치 기반)
- 캐릭터 상세 > 레벨 섹션에 인라인 편집 추가
  - 레벨(1~90) 직접 입력
  - 경험치 입력 (레벨 변경 시 해당 레벨 시작 XP 자동 입력)
  - 편집 시 레벨 XP 범위 힌트 표시
- `UpdateLevelExp` POST 액션 + 어드민 로그 기록

---

## v2.4.0 (2026-03-05)
### 인벤토리 관리
- 아이템 추가: 마스터 아이템 드롭다운 선택(이름/등급 자동 입력) 또는 직접 입력, 수량/강화/장착 설정
- 전체 추가: 마스터 아이템 전체를 인벤토리에 일괄 추가 (confirm 확인)
- 아이템 수정: 각 행의 인라인 편집 (수량·강화 input-group 라벨 표시, 장착 체크박스)
- 아이템 삭제: 행별 휴지통 버튼 + confirm
- `IGameDataService` — `GetAllItems`, `AddInventoryItem`, `AddAllItems`, `UpdateInventoryItem`, `RemoveInventoryItem` 추가
- `GameDataController` — 4개 POST 액션 + 어드민 로그

---

## v2.3.0 (2026-03-05)
### 재화 관리 (아데나 / 다이아)
- `Character` 모델에 `Diamond` 필드 추가
- MockDataStore 시드에 `Diamond = rand.Next(0, 3000)` 추가
- 캐릭터 상세 > 캐릭터 정보 카드에 재화 섹션 추가
  - 보기: 💰 아데나 / 💎 다이아 표시 + 편집 버튼 토글
  - 편집: 두 필드 인라인 폼
- `UpdateCurrency` POST 액션 + 어드민 로그

---

## v2.2.0 (2026-03-05)
### 캐릭터 상세 화면 재설계 + 스탯 편집
- 리니지M 스탯 시스템 기반 능력치 화면 재설계
  - 기본 6스탯(STR/DEX/CON/INT/WIS/CHA) 바 + 각 스탯 효과 설명
  - LFE(생명) / DTH(죽음) 스탯 추가 — CharacterStats 모델 및 DB 스키마 반영
  - HP/MP 현황 바
  - 방어도(AC) 별도 표시
  - 전투 파생 능력치 섹션 (근접/원거리/마법/생존, 읽기 전용 계산값)
- 모든 저장 스탯 13종 인라인 편집 가능: STR/DEX/CON/INT/WIS/CHA/LFE/DTH/HP/MaxHP/MP/MaxMP/AC
- `UpdateStats` POST 액션 — 변경된 값만 어드민 로그 기록
- `EF Core OwnsOne` 문제 해결: Stats 개별 프로퍼티 단위 업데이트로 변경 추적 정상화

---

## v2.1.0 (2026-03-05)
### 어드민 로그 기능
- `Models/Domain/AdminLog.cs` 신규
- `IAdminLogService` / `AdminLogService` (Mock) / `SqlAdminLogService` (EF Core)
- `AdminLogController` + `Views/AdminLog/Index.cshtml`
- 사이드바 "시스템" 섹션에 "어드민 로그" 메뉴 추가
- 주요 운영 행위 자동 기록:
  - 캐릭터 능력치/레벨/경험치/재화 수정
  - 인벤토리 추가/수정/삭제
  - 유저 제재 처리 (채팅금지/접속제한/영구정지)
  - 이벤트/공지 생성·수정·삭제
- 변경된 값만 Detail 컬럼에 기록 (예: `STR:20→25, DEX:18→22`)

---

## v2.0.0 (2026-03-05)
### EF Core + MS-SQL Server (LocalDB) 연동
- `Microsoft.EntityFrameworkCore.SqlServer` / `.Tools` / `.Design` 9.0.2 추가
- `Data/LineageMOpsDbContext.cs` 신규
  - 8개 DbSet: Accounts, BannedRecords, Characters, InventoryItems, Items, Events, Notices, ServerLogs, AdminLogs
  - `CharacterStats` → `OwnsOne` (같은 행, 컬럼 prefix `Stats_`)
  - `ApplicableServers` → JSON 직렬화 (`nvarchar(1000)`)
  - 인덱스: Account(UserId unique, Server+LastLoginAt), Character(Name+Server), ServerLog(Timestamp+Type+Server), AdminLog(CreatedAt)
- `Data/DbInitializer.cs` 신규 — `EnsureCreated()` + MockDataStore 데이터 자동 Seed
- SQL 서비스 5종: `SqlUserService`, `SqlGameDataService`, `SqlEventService`, `SqlMonitoringService`, `SqlAdminLogService`
- `appsettings.json` — `ConnectionStrings.DefaultConnection` (LocalDB), `UseMockData` 플래그
- `Program.cs` — `UseMockData` 조건부 서비스 등록 (Mock ↔ SQL 전환)
- `appsettings.json` `"Urls"` 키로 포트 설정 (`dotnet run` / `dotnet dll` 동일 적용)

---

## v1.0.0 (초기 버전)
### ASP.NET Core 9 MVC 기반 리니지M 운영 툴 — MockDataStore 인메모리 모드
- **대시보드**: 서버별 현황 카드, 실시간 접속자 Chart.js 차트, 최근 서버 로그
- **유저 관리**: 계정 검색/상세/제재 처리 (채팅금지·접속제한·영구정지)
- **게임 데이터**: 캐릭터 목록(검색·서버·직업 필터), 캐릭터 상세
- **이벤트/공지**: 탭 UI 기반 CRUD (이벤트 유형·상태, 공지 유형·발행 관리)
- **모니터링/로그**: 서버 상태 카드, 서버 로그 필터링·페이징
- `MockDataStore` Singleton — 50계정, 100+캐릭터, 아이템 마스터, 이벤트/공지, 200서버 로그 자동 생성
- Bootstrap 5 다크 테마, Bootstrap Icons, DataTables, Chart.js
