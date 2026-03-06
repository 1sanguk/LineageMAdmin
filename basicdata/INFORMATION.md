# LineageMOps — 프로그램 상세 설명

## 개요
LineageMOps는 리니지M 게임 서버 운영을 위한 **ASP.NET Core 9 MVC 기반 웹 운영 툴**이다.
게임 캐릭터 데이터 조회/수정, 계정 제재, 이벤트/공지 관리, 서버 모니터링, 어드민 로그 기록 등
실제 게임 운영에 필요한 핵심 기능을 통합 제공한다.

---

## 기술 스택

| 구분 | 기술 |
|------|------|
| 백엔드 | ASP.NET Core 9 MVC (C# 13) |
| ORM | Entity Framework Core 9.0.2 |
| DB | MS-SQL Server (LocalDB) / In-Memory Mock |
| 프론트엔드 | Bootstrap 5 (다크 테마), Bootstrap Icons |
| 차트 | Chart.js |
| 테이블 | DataTables |
| 런타임 | .NET 9 |

---

## 실행 방법

게시한 폴더로 이동 후
```bash
# 개발 실행
dotnet LineageMOps.dll
```
실행하면 디버깅 및 테스트 가능

# 포트는 appsettings.json의 "Urls" 키로 설정 (기본: http://localhost:5200)

### DB 모드 전환 (`appsettings.json`)
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=LineageMOps;Trusted_Connection=True;"
  },
  "UseMockData": false,
  "Urls": "http://localhost:5200"
}
```
- `UseMockData: true` → In-Memory MockDataStore (LocalDB 불필요, 재시작 시 데이터 초기화)
- `UseMockData: false` → EF Core + LocalDB (데이터 영속, 최초 실행 시 자동 테이블 생성 및 Mock 데이터 Seed)

---

## 프로젝트 구조

```
LineageMOps/
├── Controllers/
│   ├── DashboardController.cs       # 대시보드
│   ├── UserController.cs            # 유저/계정 관리
│   ├── GameDataController.cs        # 캐릭터/인벤토리
│   ├── EventController.cs           # 이벤트/공지
│   ├── MonitoringController.cs      # 서버 로그 모니터링
│   └── AdminLogController.cs        # 어드민 로그
│
├── Models/
│   └── Domain/
│       ├── Account.cs               # 계정 + BannedRecord
│       ├── Character.cs             # 캐릭터 + CharacterStats + InventoryItem
│       ├── Item.cs                  # 아이템 마스터
│       ├── GameEvent.cs             # 게임 이벤트
│       ├── Notice.cs                # 공지사항
│       ├── ServerLog.cs             # 서버 로그
│       ├── AdminLog.cs              # 어드민 로그
│       └── LevelTable.cs            # 레벨 1~90 누적 경험치 테이블
│
├── Services/
│   ├── IUserService.cs / UserService.cs (Mock)
│   ├── IGameDataService.cs / GameDataService.cs (Mock)
│   ├── IEventService.cs / EventService.cs (Mock)
│   ├── IMonitoringService.cs / MonitoringService.cs (Mock)
│   ├── IAdminLogService.cs / AdminLogService.cs (Mock)
│   └── Sql/
│       ├── SqlUserService.cs
│       ├── SqlGameDataService.cs
│       ├── SqlEventService.cs
│       ├── SqlMonitoringService.cs
│       └── SqlAdminLogService.cs
│
├── Data/
│   ├── MockDataStore.cs             # Singleton 인메모리 데이터 (50계정, 100+캐릭터 등)
│   ├── LineageMOpsDbContext.cs      # EF Core DbContext
│   └── DbInitializer.cs            # DB 생성 + Mock 데이터 자동 Seed
│
└── Views/
    ├── Shared/_Layout.cshtml        # 공통 레이아웃 (사이드바 네비게이션)
    ├── Dashboard/
    ├── User/
    ├── GameData/
    ├── Event/
    ├── Monitoring/
    └── AdminLog/
```

---

## 주요 기능 상세

### 1. 대시보드
- 서버별 현황 카드 (접속자 수, 상태)
- 실시간 접속자 추이 Chart.js 라인 차트
- 최근 서버 로그 목록

### 2. 유저 관리
- **계정 검색**: 아이디·이름·서버·상태 필터, 페이지네이션
- **계정 상세**: 기본 정보, 접속 이력, 제재 내역
- **제재 처리**: 채팅금지 / 접속제한 / 영구정지 — 사유 입력, 자동 어드민 로그

### 3. 게임 데이터 (캐릭터)
- **캐릭터 목록**: 이름·서버·직업 필터, 레벨 내림차순, 페이지네이션
- **캐릭터 상세**:
  - **캐릭터 정보 카드**: 직업 아이콘·색상, 레벨/경험치 바, 혈맹·생성일·최근 플레이
    - 레벨(1~90) + 경험치 인라인 편집 (레벨 변경 시 시작 XP 자동 입력, 범위 힌트 표시)
    - 재화(아데나 / 다이아) 인라인 편집
  - **능력치 카드**:
    - 기본 스탯 8종 바 표시: STR(힘) / DEX(민첩) / CON(체력) / INT(지능) / WIS(지혜) / CHA(매력) / LFE(생명) / DTH(죽음)
    - HP/MP 현황 바
    - 방어도(AC) 수치 표시
    - 전투 파생 능력치 (근접·원거리·마법·생존 그룹, 읽기 전용 계산값)
    - 편집 모드: 13종 스탯 모두 수정 가능 (STR/DEX/CON/INT/WIS/CHA/LFE/DTH/HP/MaxHP/MP/MaxMP/AC)
  - **인벤토리 카드**:
    - 아이템 목록 (이름·등급·수량·강화·장착 여부)
    - 아이템 추가: 마스터 드롭다운 선택(자동 입력) 또는 직접 입력, 수량·강화·장착 설정
    - 전체 추가: 마스터 아이템 전체 일괄 추가
    - 인라인 수정: 수량·강화(input-group 라벨 표시)·장착 변경
    - 개별 삭제 (confirm)

### 4. 이벤트 / 공지
- 탭 UI (이벤트 | 공지 전환)
- **이벤트**: 유형(경험치·드랍·로그인·홀리데이·특별), 상태(예정·진행·종료), 대상 서버, 기간
- **공지**: 유형(일반·점검·업데이트·이벤트), 발행 여부, 발행일
- CRUD 전체 지원, 수정/삭제 시 어드민 로그

### 5. 모니터링 / 로그
- 서버별 상태 카드 (MockDataStore 서버 목록 기준)
- 서버 로그 필터: 서버·로그 유형·날짜 범위, 페이지네이션
- 로그 유형: Login / Logout / Chat / Trade / System / Audit / Error

### 6. 어드민 로그
- 모든 운영 행위 자동 기록 (Action / Target / Detail / OperatorId / CreatedAt)
- Detail: 변경된 필드만 `필드명:이전값→이후값` 형식으로 기록
- 최근 100건 조회, Action별 뱃지 색상 구분

---

## 데이터 모델

### Character + CharacterStats
```
Character
├── Id, AccountId, Name, Server, Class (enum), Level, Experience, MaxExperience
├── ClanName, Adena, Diamond
├── Stats (OwnsOne → 같은 행)
│   ├── STR, DEX, CON, INT, WIS, CHA  (기본 6스탯, 최대 50)
│   ├── HP, MaxHP, MP, MaxMP
│   ├── AC (방어도, 음수일수록 높은 방어)
│   ├── LFE (생명석 — 경험치/아이템 손실 방어)
│   └── DTH (죽음석 — 대상 추가 손실 부여)
├── Inventory → List<InventoryItem>
└── CreatedAt, LastPlayedAt
```

### 레벨 테이블 (LevelTable.cs)
- 레벨 1~90 누적 경험치 임계값 90개
- `Thresholds[0..88]`: 레벨 경계 (XP < Thresholds[i] → 레벨 i+1)
- `Thresholds[89]`: 레벨 90 MaxXP (표시용)
- 레벨 90 최대 누적 XP: **10,187,168,240,849**

### 직업 (CharacterClass)
| 코드 | 이름 | 색상 |
|------|------|------|
| Knight | 군주 | #c8a951 (금) |
| DarkKnight | 기사 | #6366f1 (인디고) |
| Elf | 요정 | #22c55e (초록) |
| Wizard | 마법사 | #3b82f6 (파랑) |
| DarkElf | 다크엘프 | #a855f7 (보라) |
| Dragonknight | 드래곤나이트 | #ef4444 (빨강) |
| Illusionist | 환술사 | #ec4899 (핑크) |

---

## DB 스키마 (EF Core → SQL Server)

| 테이블 | 인덱스 |
|--------|--------|
| Accounts | UserId (unique), (Server, LastLoginAt) |
| BannedRecords | FK: AccountId |
| Characters | (Name, Server) |
| InventoryItems | FK: CharacterId |
| Items | 마스터 테이블 |
| Events | ApplicableServers (JSON→nvarchar(1000)) |
| Notices | ApplicableServers (JSON→nvarchar(1000)) |
| ServerLogs | (Timestamp, Type, Server) |
| AdminLogs | CreatedAt |

---

## MockDataStore 기본 데이터
- 계정: 50개 (서버 5개 분산)
- 캐릭터: 계정당 1~3개, 총 100+개 (레벨 1~90 랜덤)
- 아이템 마스터: 8종 (무기·방어구·소비아이템)
- 이벤트: 5개, 공지: 5개
- 서버 로그: 200개 (Login/Logout/Chat/Trade/System/Audit/Error 랜덤)
- 서버: 바츠, 기란, 아덴, 데포로탄, 글루딘

---

## 어드민 로그 기록 항목
| 액션 | 기록 조건 |
|------|-----------|
| 캐릭터 능력치 수정 | 변경된 스탯만 기록 |
| 레벨/경험치 수정 | 변경된 값만 기록 |
| 캐릭터 재화 수정 | 아데나·다이아 변경분 기록 |
| 인벤토리 아이템 추가 | 아이템명·수량·강화 기록 |
| 인벤토리 전체 추가 | 추가 종수 기록 |
| 인벤토리 아이템 수정 | ItemID·수량·강화 기록 |
| 인벤토리 아이템 삭제 | 아이템명 기록 |
| 유저 제재 처리 | 제재 유형·사유 기록 |
| 이벤트/공지 생성·수정·삭제 | 제목 기록 |
