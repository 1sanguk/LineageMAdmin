# LineageMOps

리니지M 게임 서버 운영을 위한 **ASP.NET Core 9 MVC 기반 웹 운영 툴**

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

게시하는 폴더 위치에서
```bash
dotnet LineageMOps.dll
```

포트는 `appsettings.json`의 `"Urls"` 키로 설정 (기본: `http://localhost:5200`)

### DB 모드 전환

`appsettings.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=LineageMOps;Trusted_Connection=True;"
  },
  "UseMockData": false,
  "Urls": "http://localhost:5200"
}
```

| 모드 | 설명 |
|------|------|
| `UseMockData: true` | In-Memory MockDataStore — LocalDB 불필요, 재시작 시 데이터 초기화 |
| `UseMockData: false` | EF Core + LocalDB — 데이터 영속, 최초 실행 시 자동 테이블 생성 및 Mock 데이터 Seed |

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
│   ├── AdminLogController.cs        # 어드민 로그
│   └── ClanController.cs            # 혈맹 관리
│
├── Constants/
│   ├── AppConstants.cs              # MockOperatorId 등 앱 상수
│   └── GameConstants.cs             # ServerNames, ClanConstants 등 게임 상수
│
├── Models/Domain/
│   ├── Account.cs                   # 계정
│   ├── AccountStatus.cs             # 계정 상태 enum
│   ├── BannedRecord.cs              # 제재 기록
│   ├── SanctionType.cs              # 제재 유형 enum
│   ├── Character.cs                 # 캐릭터
│   ├── CharacterClass.cs            # 직업 enum (13종)
│   ├── CharacterStats.cs            # 능력치 (13종 스탯)
│   ├── InventoryItem.cs             # 인벤토리 아이템
│   ├── ItemGrade.cs                 # 아이템 등급 enum
│   ├── Item.cs                      # 아이템 마스터
│   ├── GameEvent.cs                 # 게임 이벤트
│   ├── Notice.cs                    # 공지사항
│   ├── ServerLog.cs                 # 서버 로그
│   ├── AdminLog.cs                  # 어드민 로그
│   ├── LevelTable.cs                # 레벨 1~90 누적 경험치 테이블
│   ├── Clan.cs                      # 혈맹
│   ├── ClanMember.cs                # 혈맹원
│   ├── ClanRank.cs                  # 혈맹 계급 enum
│   ├── ClanActivityScore.cs         # 혈맹원 활동 기록 점수
│   └── JoinPolicy.cs                # 혈맹 가입 정책 enum
│
├── Services/
│   ├── IUserService.cs / UserService.cs
│   ├── IGameDataService.cs / GameDataService.cs
│   ├── IEventService.cs / EventService.cs
│   ├── IMonitoringService.cs / MonitoringService.cs
│   ├── IAdminLogService.cs / AdminLogService.cs
│   ├── IClanService.cs / ClanService.cs
│   └── Sql/
│       ├── SqlUserService.cs
│       ├── SqlGameDataService.cs
│       ├── SqlEventService.cs
│       ├── SqlMonitoringService.cs
│       └── SqlAdminLogService.cs
│
├── Models/ViewModels/
│   ├── PaginatedList.cs             # 페이지네이션 공통 모델
│   ├── CharacterDetailViewModel.cs  # 캐릭터 상세 페이지 뷰모델
│   ├── MonitoringViewModel.cs       # 모니터링 페이지 뷰모델
│   ├── DashboardViewModel.cs        # 대시보드 뷰모델
│   ├── UserDetailViewModel.cs       # 유저 상세 뷰모델
│   └── ClanDetailViewModel.cs       # 혈맹 상세 뷰모델
│
├── Data/
│   ├── MockDataStore.cs             # 인메모리 데이터 (DI Singleton)
│   ├── LineageMOpsDbContext.cs      # EF Core DbContext
│   └── DbInitializer.cs            # DB 생성 + Mock 데이터 자동 Seed
│
├── Views/
│   ├── Shared/_Layout.cshtml        # 공통 레이아웃 (사이드바)
│   ├── Dashboard/
│   ├── User/
│   ├── GameData/
│   ├── Event/
│   ├── Monitoring/
│   ├── AdminLog/
│   └── Clan/
│       ├── Index.cshtml             # 혈맹 목록 + 검색/필터
│       └── Detail.cshtml            # 혈맹 상세 (5탭)
│
└── basicdata/
    ├── GITHUB.md                    # GitHub 설정 정보
    ├── INFORMATION.md               # 프로젝트 상세 설명
    └── VERSION.md                   # 버전 히스토리
```

---

## 주요 기능

### 1. 대시보드
- 서버별 현황 카드 (접속자 수, 상태 — Online / Warning / Maintenance)
- 실시간 접속자 추이 Chart.js 라인 차트
- 최근 서버 로그 목록

### 2. 유저 관리
- **계정 검색**: 아이디·이름·서버·상태 필터, 페이지네이션
- **계정 상세**: 기본 정보, 접속 이력, 제재 내역
- **제재 처리**: 채팅금지 / 접속제한 / 영구정지 — 사유 입력, 자동 어드민 로그

### 3. 게임 데이터 (캐릭터)
- **캐릭터 목록**: 이름·서버·직업 필터, 레벨 내림차순, 페이지네이션
- **캐릭터 상세**:
  - 직업 아이콘·색상, 레벨/경험치 바, 혈맹·생성일·최근 플레이
  - 레벨(1~90) + 경험치 인라인 편집 (레벨 변경 시 시작 XP 자동 입력, 범위 힌트 표시)
  - 재화(아데나 / 다이아) 인라인 편집
  - 기본 6스탯(STR/DEX/CON/INT/WIS/CHA) + LFE/DTH 바 표시
  - HP/MP 현황 바, 방어도(AC), 전투 파생 능력치
  - 13종 스탯 전체 인라인 편집
  - 인벤토리 조회/추가/수정/삭제, 마스터 아이템 드롭다운, 전체 일괄 추가

### 4. 이벤트 / 공지
- 탭 UI (이벤트 | 공지 전환)
- **이벤트**: 유형(경험치·드랍·로그인·홀리데이·특별), 상태(예정·진행·종료), 대상 서버, 기간
- **공지**: 유형(일반·점검·업데이트·이벤트), 발행 여부, 발행일
- CRUD 전체 지원, 수정/삭제 시 어드민 로그

### 5. 모니터링 / 로그
- 서버별 상태 카드 (CPU·메모리·업타임·접속자)
- 서버 로그 필터: 서버·로그 유형·키워드, 페이지네이션
- 로그 유형: Login / Logout / Chat / Trade / System / Audit / Error

### 6. 어드민 로그
- 모든 운영 행위 자동 기록 (Action / Target / Detail / OperatorId / CreatedAt)
- 변경된 필드만 `필드명:이전값→이후값` 형식으로 기록
- 최근 100건 조회, Action별 뱃지 색상 구분

### 7. 혈맹 관리
- **혈맹 목록**: 혈맹명·혈주명·서버·레벨 필터, 명성치 내림차순, 페이지네이션
- **혈맹 상세** (5탭):
  - **기본 정보**: 현황 표시 + 레벨/명성치/경험치/전쟁 전적/아지트·성·혈맹전 편집
  - **혈맹원**: 계급별 탭(군주·수호·정예·일반·아카데미), 계급 변경 및 추방 처리
  - **활동 기록**: PVE/PVP/보스 토벌/PK/접속률/채팅/인챈트/소환/거래/인장 퀘스트 점수
  - **동맹·적대**: 적대 혈맹 / 동맹 혈맹 추가·해제 관리
  - **혈맹 설정**: 소개글, 공지사항, 피의 서약 레벨(0~6단계), 가입 정책(거부/즉시/승인/암호)
- 강제 해산 (성 보유 시 불가, 어드민 로그 기록)

---

## 데이터 모델

### 서버 목록

**레거시 월드 (20개)**
판도라, 라스타바드, 듀크데필, 블루디카, 파푸리온, 질리언, 린드비오르, 사이하, 발라카스, 군터,
안타라스, 하딘, 데스나이트, 아툰, 켄라우헬, 케레니스, 기르타스, 진기르타스, 그림리퍼, 발록

**리부트 월드 (4개)**
말하는섬, 윈다우드, 글루디오, 그레시아

### 직업 (CharacterClass)
| 코드 | 이름 | 색상 |
|------|------|------|
| Lord | 군주 | #c8a951 (금) |
| Knight | 기사 | #6366f1 (인디고) |
| Elf | 요정 | #22c55e (초록) |
| Wizard | 마법사 | #3b82f6 (파랑) |
| DarkElf | 다크엘프 | #a855f7 (보라) |
| Gunner | 총사 | #f97316 (오렌지) |
| Fighter | 투사 | #ef4444 (빨강) |
| DarkKnight | 암흑기사 | #7c3aed (진보라) |
| HolyKnight | 신성검사 | #fbbf24 (앰버) |
| Berserker | 광전사 | #dc2626 (진홍) |
| Reaper | 사신 | #6b7280 (회색) |
| ThunderGod | 뇌신 | #06b6d4 (시안) |
| SpellBlade | 마검사 | #8b5cf6 (바이올렛) |

### CharacterStats
| 스탯 | 설명 |
|------|------|
| STR | 힘 — 근접 공격력 |
| DEX | 민첩 — 명중·회피 |
| CON | 체력 — HP·방어 |
| INT | 지능 — 마법 공격력 |
| WIS | 지혜 — MP·마법 저항 |
| CHA | 매력 — 거래·설득 |
| LFE | 생명석 — 경험치/아이템 손실 방어 |
| DTH | 죽음석 — 대상 추가 손실 부여 |
| AC | 방어도 (음수일수록 높은 방어) |

### 레벨 테이블
- 레벨 1~90 누적 경험치 임계값 (실제 리니지M 수치 기반)
- 레벨 90 최대 누적 XP: **10,187,168,240,849**

### 혈맹 (Clan)
| 필드 | 설명 |
|------|------|
| Level | 혈맹 레벨 (1~10) |
| Reputation | 명성치 |
| Experience | 혈맹 경험치 |
| BloodOathLevel | 피의 서약 레벨 (0=미개방, 1~6단계, 레벨당 최대인원 +5) |
| JoinPolicy | 가입 정책 (Closed/Immediate/Approval/Password) |
| Introduction | 혈맹 소개글 |
| Notice | 혈맹 공지사항 |
| RivalClanNames | 적대 혈맹 목록 |
| AllyClanNames | 동맹 혈맹 목록 |

### 혈맹 계급 (ClanRank)
| 코드 | 이름 | 설명 |
|------|------|------|
| Leader | 군주 | 모든 혈맹 권한 보유 |
| Guardian | 수호 | 군주 보좌, 핵심 권한 공유 가능 |
| Elite | 정예 | 피의 서약으로 슬롯 확장 가능 |
| Regular | 일반 | 기본 혈맹 활동 |
| Academy | 아카데미 | 레벨 40 미만, 졸업 시 일반 계급 |

### 혈맹원 활동 기록 (ClanActivityScore)
PVE / PVP / 보스 토벌 / PK / 접속률(최근 30일) / 채팅 / 인챈트 / 소환 / 거래 / 인장 퀘스트

### DB 스키마 (EF Core → SQL Server)
| 테이블 | 인덱스 |
|--------|--------|
| Accounts | UserId (unique), (Server, LastLoginAt) |
| BannedRecords | FK: AccountId |
| Characters | (Name, Server) |
| InventoryItems | FK: CharacterId |
| Items | 마스터 테이블 |
| Events | ApplicableServers (JSON → nvarchar(1000)) |
| Notices | ApplicableServers (JSON → nvarchar(1000)) |
| ServerLogs | (Timestamp, Type, Server) |
| AdminLogs | CreatedAt |

> **참고**: 혈맹(Clan) 관련 테이블은 현재 Mock 전용 구현입니다. SQL 모드 지원 예정.

### MockDataStore 기본 데이터
| 항목 | 수량 |
|------|------|
| 계정 | 50개 (전체 24개 서버 분산) |
| 캐릭터 | 계정당 1~3개, 총 100+개 |
| 아이템 마스터 | 8종 |
| 이벤트 | 5개 |
| 공지 | 5개 |
| 서버 로그 | 200개 |
| 혈맹 | 5개 (레벨 4~10, 적대·동맹 관계 포함) |
