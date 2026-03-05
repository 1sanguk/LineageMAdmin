# LineageMOps — 버전 히스토리

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
