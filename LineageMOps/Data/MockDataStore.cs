using LineageMOps.Constants;
using LineageMOps.Models.Domain;

namespace LineageMOps.Data;

public class MockDataStore
{
    public List<Account> Accounts { get; private set; }
    public List<Character> Characters { get; private set; }
    public List<Item> Items { get; private set; }
    public List<GameEvent> Events { get; private set; }
    public List<Notice> Notices { get; private set; }
    public List<ServerLog> Logs { get; private set; }
    public List<ServerStatus> Servers { get; private set; }
    public List<AdminLog> AdminLogs { get; private set; } = new();
    public List<Clan> Clans { get; private set; }

    public MockDataStore()
    {
        Servers = SeedServers();
        Accounts = SeedAccounts();
        Characters = SeedCharacters();
        Items = SeedItems();
        Events = SeedEvents();
        Notices = SeedNotices();
        Logs = SeedLogs();
        Clans = SeedClans();
    }

    private static List<ServerStatus> SeedServers()
    {
        // 레거시 월드
        return new List<ServerStatus>
        {
            new() { Name = "pandora",      DisplayName = "판도라",    State = ServerState.Online,      CurrentPlayers = 5821, MaxPlayers = 8000, CpuUsage = 52.3, MemoryUsage = 68.7, Uptime = 99.95 },
            new() { Name = "rastabaad",    DisplayName = "라스타바드",State = ServerState.Online,      CurrentPlayers = 6103, MaxPlayers = 8000, CpuUsage = 58.1, MemoryUsage = 73.2, Uptime = 99.95 },
            new() { Name = "dukedevil",    DisplayName = "듀크데필",  State = ServerState.Online,      CurrentPlayers = 4502, MaxPlayers = 8000, CpuUsage = 47.3, MemoryUsage = 62.1, Uptime = 99.98 },
            new() { Name = "blrudika",     DisplayName = "블루디카",  State = ServerState.Online,      CurrentPlayers = 3800, MaxPlayers = 8000, CpuUsage = 41.5, MemoryUsage = 55.4, Uptime = 99.99 },
            new() { Name = "papurion",     DisplayName = "파푸리온",  State = ServerState.Online,      CurrentPlayers = 5200, MaxPlayers = 8000, CpuUsage = 49.8, MemoryUsage = 65.3, Uptime = 99.97 },
            new() { Name = "jillian",      DisplayName = "질리언",    State = ServerState.Warning,     CurrentPlayers = 7654, MaxPlayers = 8000, CpuUsage = 87.4, MemoryUsage = 86.6, Uptime = 99.87 },
            new() { Name = "lindvior",     DisplayName = "린드비오르",State = ServerState.Online,      CurrentPlayers = 4098, MaxPlayers = 8000, CpuUsage = 45.6, MemoryUsage = 59.1, Uptime = 99.96 },
            new() { Name = "saiha",        DisplayName = "사이하",    State = ServerState.Online,      CurrentPlayers = 3241, MaxPlayers = 8000, CpuUsage = 31.2, MemoryUsage = 48.9, Uptime = 100.0  },
            new() { Name = "valakas",      DisplayName = "발라카스",  State = ServerState.Online,      CurrentPlayers = 5512, MaxPlayers = 8000, CpuUsage = 52.8, MemoryUsage = 67.4, Uptime = 99.99 },
            new() { Name = "gunter",       DisplayName = "군터",      State = ServerState.Online,      CurrentPlayers = 4821, MaxPlayers = 8000, CpuUsage = 42.3, MemoryUsage = 61.7, Uptime = 99.98 },
            new() { Name = "antharas",     DisplayName = "안타라스",  State = ServerState.Online,      CurrentPlayers = 5341, MaxPlayers = 8000, CpuUsage = 55.1, MemoryUsage = 70.2, Uptime = 99.94 },
            new() { Name = "hardin",       DisplayName = "하딘",      State = ServerState.Maintenance, CurrentPlayers = 0,    MaxPlayers = 8000, CpuUsage = 5.1,  MemoryUsage = 22.3, Uptime = 98.21 },
            new() { Name = "deathknight",  DisplayName = "데스나이트",State = ServerState.Online,      CurrentPlayers = 4600, MaxPlayers = 8000, CpuUsage = 48.2, MemoryUsage = 63.8, Uptime = 99.96 },
            new() { Name = "atun",         DisplayName = "아툰",      State = ServerState.Online,      CurrentPlayers = 3950, MaxPlayers = 8000, CpuUsage = 43.7, MemoryUsage = 57.2, Uptime = 99.97 },
            new() { Name = "kenrauhel",    DisplayName = "켄라우헬",  State = ServerState.Online,      CurrentPlayers = 4821, MaxPlayers = 8000, CpuUsage = 42.3, MemoryUsage = 61.7, Uptime = 99.98 },
            new() { Name = "kerenice",     DisplayName = "케레니스",  State = ServerState.Online,      CurrentPlayers = 5100, MaxPlayers = 8000, CpuUsage = 50.4, MemoryUsage = 66.1, Uptime = 99.95 },
            new() { Name = "girtas",       DisplayName = "기르타스",  State = ServerState.Online,      CurrentPlayers = 4350, MaxPlayers = 8000, CpuUsage = 46.8, MemoryUsage = 60.3, Uptime = 99.97 },
            new() { Name = "jingirtas",    DisplayName = "진기르타스",State = ServerState.Online,      CurrentPlayers = 3780, MaxPlayers = 8000, CpuUsage = 40.9, MemoryUsage = 54.7, Uptime = 99.99 },
            new() { Name = "grimreaper",   DisplayName = "그림리퍼",  State = ServerState.Online,      CurrentPlayers = 4920, MaxPlayers = 8000, CpuUsage = 51.2, MemoryUsage = 67.0, Uptime = 99.96 },
            new() { Name = "balog",        DisplayName = "발록",      State = ServerState.Online,      CurrentPlayers = 5680, MaxPlayers = 8000, CpuUsage = 56.3, MemoryUsage = 71.5, Uptime = 99.93 },
            // 리부트 월드
            new() { Name = "talkingisland",DisplayName = "말하는섬",  State = ServerState.Online,      CurrentPlayers = 2500, MaxPlayers = 5000, CpuUsage = 35.2, MemoryUsage = 52.1, Uptime = 99.99 },
            new() { Name = "windwood",     DisplayName = "윈다우드",  State = ServerState.Online,      CurrentPlayers = 3100, MaxPlayers = 5000, CpuUsage = 38.7, MemoryUsage = 55.8, Uptime = 99.98 },
            new() { Name = "gludio",       DisplayName = "글루디오",  State = ServerState.Online,      CurrentPlayers = 2800, MaxPlayers = 5000, CpuUsage = 36.5, MemoryUsage = 53.4, Uptime = 99.99 },
            new() { Name = "grecia",       DisplayName = "그레시아",  State = ServerState.Online,      CurrentPlayers = 2200, MaxPlayers = 5000, CpuUsage = 32.1, MemoryUsage = 49.6, Uptime = 100.0  },
        };
    }

    private static List<Account> SeedAccounts()
    {
        var accounts = new List<Account>();
        var rand = new Random(42);
        var firstNames = new[] { "김", "이", "박", "최", "정", "강", "조", "윤", "장", "임" };
        var lastNames = new[] { "민준", "서준", "예준", "도윤", "시우", "지호", "지후", "준서", "준우", "현우", "지유", "서연", "서윤", "지아", "하은", "하윤", "민서", "채원" };
        var statuses = new[] { AccountStatus.Active, AccountStatus.Active, AccountStatus.Active, AccountStatus.Suspended, AccountStatus.Banned, AccountStatus.Dormant };

        for (int i = 1; i <= 50; i++)
        {
            var regDate = DateTime.Now.AddDays(-rand.Next(30, 1800));
            var status = statuses[rand.Next(statuses.Length)];
            var account = new Account
            {
                Id = i,
                UserId = $"user{i:D4}",
                UserName = $"{firstNames[rand.Next(firstNames.Length)]}{lastNames[rand.Next(lastNames.Length)]}",
                Email = $"user{i:D4}@email.com",
                Server = GameConstants.ServerNames[rand.Next(GameConstants.ServerNames.Length)],
                RegisteredAt = regDate,
                LastLoginAt = regDate.AddDays(rand.Next(1, (int)(DateTime.Now - regDate).TotalDays)),
                IpAddress = $"192.168.{rand.Next(1, 255)}.{rand.Next(1, 255)}",
                Status = status,
                Sanctions = new List<BannedRecord>()
            };

            if (status == AccountStatus.Suspended || status == AccountStatus.Banned || rand.Next(10) < 2)
                AddSanctionToAccount(account, rand);

            accounts.Add(account);
        }
        return accounts;
    }

    private static void AddSanctionToAccount(Account account, Random rand)
    {
        var sanctionType = account.Status == AccountStatus.Banned ? SanctionType.PermanentBan :
                           rand.Next(2) == 0 ? SanctionType.ChatBan : SanctionType.LoginRestriction;
        var startDate = account.RegisteredAt.AddDays(rand.Next(1, 100));
        account.Sanctions.Add(new BannedRecord
        {
            Id = account.Id * 10,
            AccountId = account.Id,
            Type = sanctionType,
            Reason = GetSanctionReason(sanctionType, rand),
            StartDate = startDate,
            EndDate = sanctionType == SanctionType.PermanentBan ? null : startDate.AddDays(rand.Next(1, 30)),
            OperatorId = $"op_{rand.Next(1, 5):D3}",
            IsActive = account.Status != AccountStatus.Active
        });
    }

    private static string GetSanctionReason(SanctionType type, Random rand)
    {
        var chatReasons = new[] { "욕설 및 비속어 사용", "도배 행위", "타 유저 비방", "광고성 채팅" };
        var loginReasons = new[] { "계정 공유 의심", "핵/봇 사용 의심", "현금 거래 신고" };
        var banReasons = new[] { "핵 프로그램 사용 확인", "계정 도용", "환불 사기", "반복적 규정 위반" };
        return type switch
        {
            SanctionType.ChatBan => chatReasons[rand.Next(chatReasons.Length)],
            SanctionType.LoginRestriction => loginReasons[rand.Next(loginReasons.Length)],
            SanctionType.PermanentBan => banReasons[rand.Next(banReasons.Length)],
            _ => "규정 위반"
        };
    }

    private List<Character> SeedCharacters()
    {
        var chars = new List<Character>();
        var rand = new Random(42);
        var charClasses = Enum.GetValues<CharacterClass>();
        var clanNames = new[] { "켄라우헬의용군", "켄라우헬기사단", "흑의기사단", "용사의동맹", "붉은달", "", "", "" };
        int id = 1;

        foreach (var account in Accounts)
        {
            int charCount = rand.Next(1, 4);
            for (int c = 0; c < charCount; c++)
            {
                var cls = charClasses[rand.Next(charClasses.Length)];
                var level = rand.Next(1, LevelTable.MaxLevel + 1);
                var character = new Character
                {
                    Id = id++,
                    AccountId = account.Id,
                    Name = GenerateCharName(rand),
                    Server = account.Server,
                    Class = cls,
                    Level = level,
                    MaxExperience = LevelTable.GetMaxXP(level),
                    ClanName = clanNames[rand.Next(clanNames.Length)],
                    Adena = rand.Next(10000, 50000000),
                    Diamond = rand.Next(0, 3000),
                    CreatedAt = account.RegisteredAt.AddDays(rand.Next(0, 10)),
                    LastPlayedAt = account.LastLoginAt,
                    Stats = GenerateStats(cls, level, rand)
                };
                character.Experience = rand.NextInt64(0, character.MaxExperience);
                character.Inventory = GenerateInventory(character.Id, rand);
                chars.Add(character);
            }
        }
        return chars;
    }

    private static string GenerateCharName(Random rand)
    {
        var prefixes = new[] { "다크", "불꽃", "폭풍", "그림자", "빛의", "어둠의", "철벽", "황금", "붉은" };
        var suffixes = new[] { "기사", "영웅", "전사", "마법사", "사냥꾼", "수호자", "파괴자", "지배자" };
        return $"{prefixes[rand.Next(prefixes.Length)]}{suffixes[rand.Next(suffixes.Length)]}{rand.Next(10, 99)}";
    }

    private static CharacterStats GenerateStats(CharacterClass cls, int level, Random rand)
    {
        int baseHp = level * 50 + rand.Next(100, 500);
        int baseMp = level * 20 + rand.Next(50, 200);
        return new CharacterStats
        {
            Str = rand.Next(8, 18) + cls switch
            {
                CharacterClass.Knight or CharacterClass.Fighter or CharacterClass.Berserker => 4,
                CharacterClass.Lord or CharacterClass.DarkKnight or CharacterClass.HolyKnight or CharacterClass.SpellBlade => 2,
                CharacterClass.DarkElf or CharacterClass.Gunner => 2,
                _ => 0
            },
            Dex = rand.Next(8, 18) + cls switch
            {
                CharacterClass.Elf or CharacterClass.DarkElf or CharacterClass.Reaper => 3,
                CharacterClass.Gunner => 4,
                _ => 0
            },
            Con = rand.Next(8, 18) + cls switch
            {
                CharacterClass.Knight => 3,
                CharacterClass.Lord or CharacterClass.Fighter => 2,
                _ => 0
            },
            Wis = rand.Next(8, 18) + cls switch
            {
                CharacterClass.Wizard or CharacterClass.ThunderGod => 2,
                CharacterClass.Elf or CharacterClass.HolyKnight => 3,
                _ => 0
            },
            Int = rand.Next(8, 18) + cls switch
            {
                CharacterClass.Wizard => 4,
                CharacterClass.ThunderGod or CharacterClass.SpellBlade => 3,
                _ => 0
            },
            Cha = rand.Next(8, 18) + (cls == CharacterClass.Lord ? 4 : 0),
            Hp = (int)(baseHp * 0.7),
            MaxHp = baseHp,
            Mp = (int)(baseMp * 0.8),
            MaxMp = baseMp,
            Ac = rand.Next(-30, 10),
            Lfe = rand.Next(0, 8) + (cls == CharacterClass.HolyKnight ? 2 : 0),
            Dth = rand.Next(0, 5) + (cls is CharacterClass.DarkKnight or CharacterClass.Reaper ? 2 : 0)
        };
    }

    private static List<InventoryItem> GenerateInventory(int charId, Random rand)
    {
        var items = new List<InventoryItem>
        {
            CreateWeaponItem(charId, rand),
            CreateArmorItem(charId, rand)
        };
        items.AddRange(CreateConsumableItems(charId, rand));
        return items;
    }

    private static InventoryItem CreateWeaponItem(int charId, Random rand)
    {
        var weaponNames = new[] { "엘더 블레이드", "그림자의 단검", "마법사의 지팡이", "드워프 도끼", "신성한 활", "고룡의 검" };
        return new InventoryItem
        {
            Id = charId * 100 + 1,
            CharacterId = charId,
            ItemId = rand.Next(1, 20),
            ItemName = weaponNames[rand.Next(weaponNames.Length)],
            Grade = (ItemGrade)rand.Next(0, 5),
            Quantity = 1,
            IsEquipped = true,
            Enchant = rand.Next(0, 12)
        };
    }

    private static InventoryItem CreateArmorItem(int charId, Random rand)
    {
        var armorNames = new[] { "강철 갑옷", "그림자 로브", "요정 갑옷", "용린 방어구", "흑철 갑옷" };
        return new InventoryItem
        {
            Id = charId * 100 + 2,
            CharacterId = charId,
            ItemId = rand.Next(20, 40),
            ItemName = armorNames[rand.Next(armorNames.Length)],
            Grade = (ItemGrade)rand.Next(0, 4),
            Quantity = 1,
            IsEquipped = true,
            Enchant = rand.Next(0, 8)
        };
    }

    private static List<InventoryItem> CreateConsumableItems(int charId, Random rand)
    {
        var consumableNames = new[] { "회복 포션", "마나 포션", "비약", "스크롤", "귀환 주문서" };
        var items = new List<InventoryItem>();
        int count = rand.Next(2, 6);
        for (int i = 0; i < count; i++)
        {
            items.Add(new InventoryItem
            {
                Id = charId * 100 + 3 + i,
                CharacterId = charId,
                ItemId = rand.Next(50, 70),
                ItemName = consumableNames[rand.Next(consumableNames.Length)],
                Grade = ItemGrade.Normal,
                Quantity = rand.Next(1, 100),
                IsEquipped = false,
                Enchant = 0
            });
        }
        return items;
    }

    private static List<Item> SeedItems()
    {
        return new List<Item>
        {
            new() { Id = 1, Name = "엘더 블레이드",   Type = ItemType.Weapon,     Grade = ItemGrade.Epic,      Description = "고대 기사의 검",              Weight = 1540, BuyPrice = 5000000,  SellPrice = 1250000, IsTradeble = true },
            new() { Id = 2, Name = "그림자의 단검",   Type = ItemType.Weapon,     Grade = ItemGrade.Rare,      Description = "다크엘프 암살자의 단검",       Weight = 520,  BuyPrice = 1200000,  SellPrice = 300000,  IsTradeble = true },
            new() { Id = 3, Name = "마법사의 지팡이", Type = ItemType.Weapon,     Grade = ItemGrade.Rare,      Description = "마력이 깃든 지팡이",           Weight = 420,  BuyPrice = 900000,   SellPrice = 225000,  IsTradeble = true },
            new() { Id = 4, Name = "강철 갑옷",       Type = ItemType.Armor,      Grade = ItemGrade.Magic,     Description = "견고한 강철 갑옷",             Weight = 3200, BuyPrice = 800000,   SellPrice = 200000,  IsTradeble = true },
            new() { Id = 5, Name = "고룡의 검",       Type = ItemType.Weapon,     Grade = ItemGrade.Legendary, Description = "전설의 용 사냥꾼이 사용했던 검", Weight = 1800, BuyPrice = 50000000, SellPrice = 12500000,IsTradeble = false },
            new() { Id = 6, Name = "회복 포션",       Type = ItemType.Consumable, Grade = ItemGrade.Normal,    Description = "HP를 500 회복",               Weight = 50,   BuyPrice = 500,      SellPrice = 50,      IsTradeble = true },
            new() { Id = 7, Name = "마나 포션",       Type = ItemType.Consumable, Grade = ItemGrade.Normal,    Description = "MP를 300 회복",               Weight = 50,   BuyPrice = 800,      SellPrice = 80,      IsTradeble = true },
            new() { Id = 8, Name = "귀환 주문서",     Type = ItemType.Consumable, Grade = ItemGrade.Normal,    Description = "마을로 귀환",                  Weight = 20,   BuyPrice = 1000,     SellPrice = 100,     IsTradeble = true },
        };
    }

    private List<GameEvent> SeedEvents()
    {
        var now = DateTime.Now;
        var allServers = GameConstants.ServerNames.ToList();
        return new List<GameEvent>
        {
            new() { Id = 1, Title = "봄맞이 경험치 2배 이벤트",     Type = EventType.Exp,     Status = EventStatus.Active,     StartDate = now.AddDays(-5),  EndDate = now.AddDays(9),  ApplicableServers = allServers,                                    CreatedBy = "op_001", CreatedAt = now.AddDays(-10), Description = "봄을 맞이하여 모든 서버에서 경험치 2배 이벤트를 진행합니다. 이 기간 동안 획득하는 모든 경험치가 2배로 적용됩니다." },
            new() { Id = 2, Title = "드랍률 UP! 보물 사냥 이벤트",  Type = EventType.Drop,    Status = EventStatus.Active,     StartDate = now.AddDays(-2),  EndDate = now.AddDays(5),  ApplicableServers = new List<string> { "켄라우헬", "안타라스", "발라카스" }, CreatedBy = "op_002", CreatedAt = now.AddDays(-7),  Description = "일주일간 아이템 드랍률이 1.5배로 증가합니다. 레어 아이템을 노려보세요!" },
            new() { Id = 3, Title = "주간 로그인 보상 이벤트",      Type = EventType.Login,   Status = EventStatus.Scheduled,  StartDate = now.AddDays(3),   EndDate = now.AddDays(17), ApplicableServers = allServers,                                    CreatedBy = "op_001", CreatedAt = now.AddDays(-1),  Description = "매일 접속 시 특별 보상 아이템을 지급합니다. 7일 연속 접속 시 추가 보상!" },
            new() { Id = 4, Title = "신년 특별 이벤트",             Type = EventType.Holiday, Status = EventStatus.Ended,      StartDate = now.AddDays(-60), EndDate = now.AddDays(-46),ApplicableServers = allServers,                                    CreatedBy = "op_003", CreatedAt = now.AddDays(-65), Description = "새해를 맞이하여 진행된 특별 이벤트입니다." },
            new() { Id = 5, Title = "혈맹 토너먼트",                Type = EventType.Special, Status = EventStatus.Scheduled,  StartDate = now.AddDays(14),  EndDate = now.AddDays(21), ApplicableServers = new List<string> { "켄라우헬", "안타라스" },  CreatedBy = "op_001", CreatedAt = now,              Description = "서버 최강 혈맹을 가리는 토너먼트입니다. 우승 혈맹에게는 특별 칭호와 보상을 지급합니다." },
        };
    }

    private List<Notice> SeedNotices()
    {
        var now = DateTime.Now;
        var allServers = GameConstants.ServerNames.ToList();
        return new List<Notice>
        {
            new() { Id = 1, Title = "[긴급] 질리언 서버 긴급 점검 안내",         Type = NoticeType.Maintenance, IsPublished = true,  PublishDate = now.AddHours(-2),  ApplicableServers = new List<string> { "질리언" }, CreatedBy = "op_001", CreatedAt = now.AddHours(-3),  Content = "질리언 서버에서 일부 유저들이 접속 불안정을 겪고 있어 긴급 점검을 진행합니다.\n\n■ 점검 일시: 2026-03-04 02:00 ~ 04:00 (2시간)\n■ 점검 서버: 질리언\n■ 점검 내용: 서버 과부하 원인 조사 및 안정화 작업\n\n점검 중 접속 불가 및 진행 중인 전투가 종료될 수 있습니다." },
            new() { Id = 2, Title = "[업데이트] 3월 정기 업데이트 안내",          Type = NoticeType.Update,      IsPublished = true,  PublishDate = now.AddDays(-1),   ApplicableServers = allServers,                 CreatedBy = "op_002", CreatedAt = now.AddDays(-2),   Content = "2026년 3월 정기 업데이트 내용을 안내드립니다.\n\n■ 신규 사냥터: 공허의 탑 9~12층 추가\n■ 신규 아이템: 전설 등급 무기 3종 추가\n■ 밸런스 패치: 군주 직업 스킬 조정\n■ 버그 수정: 특정 상황에서 아이템이 사라지는 문제 수정" },
            new() { Id = 3, Title = "[이벤트] 봄맞이 경험치 2배 이벤트 안내",    Type = NoticeType.Event,       IsPublished = true,  PublishDate = now.AddDays(-5),   ApplicableServers = allServers,                 CreatedBy = "op_001", CreatedAt = now.AddDays(-6),   Content = "봄을 맞이하여 경험치 2배 이벤트를 진행합니다!\n\n■ 이벤트 기간: 2026-02-28 ~ 2026-03-13\n■ 적용 서버: 전 서버\n■ 이벤트 내용: 모든 경험치 2배 획득" },
            new() { Id = 4, Title = "[공지] 불법 프로그램 사용 제재 강화 안내",   Type = NoticeType.General,     IsPublished = true,  PublishDate = now.AddDays(-7),   ApplicableServers = allServers,                 CreatedBy = "op_003", CreatedAt = now.AddDays(-8),   Content = "최근 불법 자동 사냥 프로그램 사용자가 증가함에 따라 제재를 강화합니다.\n\n■ 1차 적발: 7일 접속 제한\n■ 2차 적발: 30일 접속 제한\n■ 3차 적발: 영구 정지\n\n공정한 게임 환경을 위해 불법 프로그램 사용을 자제해 주시기 바랍니다." },
            new() { Id = 5, Title = "[예정] 4월 대규모 업데이트 예고",            Type = NoticeType.Update,      IsPublished = false, PublishDate = now.AddDays(10),   ApplicableServers = allServers,                 CreatedBy = "op_001", CreatedAt = now,               Content = "4월에 예정된 대규모 업데이트를 예고합니다. 상세 내용은 추후 공지 예정입니다." },
        };
    }

    private List<ServerLog> SeedLogs()
    {
        var logs = new List<ServerLog>();
        var rand = new Random(99);
        var now = DateTime.Now;
        var loginUsers = Accounts.Take(20).ToList();
        var chatMessages = new[] { "거래 합니다", "파티 구합니다", "아이템 팝니다", "퀘스트 도움 요청", "혈맹원 모집" };
        var systemMessages = new[] { "서버 백업 완료", "메모리 최적화 실행", "DB 연결 풀 갱신", "캐시 클리어 완료" };
        var auditMessages = new[] { "제재 처리 완료", "공지 등록", "이벤트 활성화", "계정 조회", "캐릭터 데이터 수정" };

        for (int i = 0; i < 200; i++)
        {
            logs.Add(CreateLogEntry(
                id: i + 1,
                server: GameConstants.ServerNames[rand.Next(GameConstants.ServerNames.Length)],
                logType: (LogType)rand.Next(0, 7),
                account: loginUsers[rand.Next(loginUsers.Count)],
                rand: rand,
                now: now,
                chatMessages: chatMessages,
                systemMessages: systemMessages,
                auditMessages: auditMessages
            ));
        }

        return logs.OrderByDescending(l => l.Timestamp).ToList();
    }

    private static ServerLog CreateLogEntry(int id, string server, LogType logType, Account account, Random rand, DateTime now,
        string[] chatMessages, string[] systemMessages, string[] auditMessages)
    {
        var timestamp = now.AddMinutes(-rand.Next(0, 1440));
        return new ServerLog
        {
            Id = id,
            Server = server,
            Type = logType,
            Level = logType == LogType.Error ? Models.Domain.LogLevel.Error :
                    logType == LogType.System ? Models.Domain.LogLevel.Warning : Models.Domain.LogLevel.Info,
            Message = logType switch
            {
                LogType.Login => $"유저 [{account.UserId}] 접속",
                LogType.Logout => $"유저 [{account.UserId}] 접속 종료",
                LogType.Chat => $"[{account.UserId}] {chatMessages[rand.Next(chatMessages.Length)]}",
                LogType.Trade => $"[{account.UserId}] 아이템 거래 완료 (아데나 {rand.Next(1000, 5000000):N0})",
                LogType.System => systemMessages[rand.Next(systemMessages.Length)],
                LogType.Audit => $"운영자 [op_{rand.Next(1, 4):D3}] {auditMessages[rand.Next(auditMessages.Length)]}",
                LogType.Error => $"오류 발생: {(rand.Next(2) == 0 ? "DB 연결 실패" : "스킬 처리 오류")}",
                _ => "알 수 없는 이벤트"
            },
            UserId = logType is LogType.Login or LogType.Logout or LogType.Chat or LogType.Trade ? account.UserId : null,
            Timestamp = timestamp,
            IpAddress = logType is LogType.Login ? account.IpAddress : null,
            Detail = logType == LogType.Error ? $"ErrorCode: E{rand.Next(1000, 9999)}" : null
        };
    }

    // Mutating helpers
    public void AddSanction(BannedRecord sanction)
    {
        var account = Accounts.FirstOrDefault(a => a.Id == sanction.AccountId);
        if (account == null) return;
        sanction.Id = Accounts.SelectMany(a => a.Sanctions).Any()
            ? Accounts.SelectMany(a => a.Sanctions).Max(s => s.Id) + 1
            : 1;
        account.Sanctions.Add(sanction);
        account.Status = sanction.Type == SanctionType.PermanentBan ? AccountStatus.Banned : AccountStatus.Suspended;
    }

    public void SaveEvent(GameEvent evt)
    {
        var existing = Events.FirstOrDefault(e => e.Id == evt.Id);
        if (existing != null)
            Events.Remove(existing);
        else
            evt.Id = Events.Any() ? Events.Max(e => e.Id) + 1 : 1;
        Events.Add(evt);
    }

    public void DeleteEvent(int id) => Events.RemoveAll(e => e.Id == id);

    public void SaveNotice(Notice notice)
    {
        var existing = Notices.FirstOrDefault(n => n.Id == notice.Id);
        if (existing != null)
            Notices.Remove(existing);
        else
            notice.Id = Notices.Any() ? Notices.Max(n => n.Id) + 1 : 1;
        Notices.Add(notice);
    }

    public void DeleteNotice(int id) => Notices.RemoveAll(n => n.Id == id);

    private List<Clan> SeedClans()
    {
        var rand = new Random(77);

        // 미리 인덱싱 — Characters 전체를 clan마다 반복 스캔하지 않음
        var charsByClanName = Characters
            .Where(c => !string.IsNullOrEmpty(c.ClanName))
            .GroupBy(c => c.ClanName)
            .ToDictionary(g => g.Key, g => g.OrderByDescending(c => c.Level).ToList());

        // 아카데미 후보 풀 — 한 번만 계산, 중복 배정 방지를 위해 순서대로 소비
        var academyPool = Characters
            .Where(c => c.Level < ClanConstants.AcademyGraduationLevel && string.IsNullOrEmpty(c.ClanName))
            .OrderBy(c => c.Id)
            .ToList();
        int academyPoolIndex = 0;

        var clanDefs = GetClanDefinitions();
        var clans    = new List<Clan>();
        int clanId   = 1;

        foreach (var def in clanDefs)
        {
            if (!charsByClanName.TryGetValue(def.Name, out var matchingChars)) continue;

            var leader    = matchingChars.First();
            var clanStart = leader.CreatedAt.AddDays(-rand.Next(30, 180));
            var members   = BuildClanMembers(matchingChars, clanStart, rand);

            if (def.Level >= ClanConstants.AcademyMinLevel)
            {
                int count = rand.Next(3, 8);
                AddAcademyMembers(members, academyPool, ref academyPoolIndex, count, clanStart, rand);
            }

            clans.Add(new Clan
            {
                Id                = clanId++,
                Name              = def.Name,
                Server            = def.Server,
                Level             = def.Level,
                Reputation        = def.Reputation,
                Experience        = def.Experience,
                LeaderCharacterId = leader.Id,
                LeaderName        = leader.Name,
                WarWins           = def.Wins,
                WarLosses         = def.Losses,
                HasAjit           = def.HasAjit,
                HasCastle         = def.HasCastle,
                IsAtWar           = def.IsAtWar,
                BloodOathLevel    = def.BloodOathLevel,
                JoinPolicy        = def.JoinPolicy,
                Introduction      = def.Introduction,
                Notice            = def.Notice,
                Emblem            = def.Emblem,
                RivalClanNames    = def.Rivals.ToList(),
                AllyClanNames     = def.Allies.ToList(),
                CreatedAt         = clanStart,
                Members           = members
            });
        }

        return clans;
    }

    private static List<ClanMember> BuildClanMembers(List<Character> chars, DateTime clanStart, Random rand)
        => chars.Select((ch, idx) => new ClanMember
        {
            CharacterId   = ch.Id,
            CharacterName = ch.Name,
            ClassName     = GameConstants.GetClassName(ch.Class),
            Level         = ch.Level,
            Rank          = DetermineRank(idx),
            JoinedAt      = clanStart.AddDays(rand.Next(0, 60)),
            LastOnlineAt  = ch.LastPlayedAt,
            IsOnline      = rand.Next(5) == 0,
            ActivityScore = GenerateActivityScore(ch.Level, rand)
        }).ToList();

    private static void AddAcademyMembers(
        List<ClanMember> members, List<Character> pool,
        ref int poolIndex, int count, DateTime clanStart, Random rand)
    {
        int taken = 0;
        while (taken < count && poolIndex < pool.Count)
        {
            var ac = pool[poolIndex++];
            members.Add(new ClanMember
            {
                CharacterId   = ac.Id,
                CharacterName = ac.Name,
                ClassName     = GameConstants.GetClassName(ac.Class),
                Level         = ac.Level,
                Rank          = ClanRank.Academy,
                JoinedAt      = clanStart.AddDays(rand.Next(30, 120)),
                LastOnlineAt  = ac.LastPlayedAt,
                IsOnline      = false,
                ActivityScore = GenerateActivityScore(ac.Level, rand)
            });
            taken++;
        }
    }

    private static ClanRank DetermineRank(int idx) => idx switch
    {
        0    => ClanRank.Leader,
        1    => ClanRank.Guardian,
        <= 4 => ClanRank.Elite,
        _    => ClanRank.Regular
    };

    private static ClanActivityScore GenerateActivityScore(int level, Random rand)
    {
        int scale = Math.Max(1, level / 15);
        return new ClanActivityScore
        {
            Pve       = rand.Next(0, 500)  * scale,
            Pvp       = rand.Next(0, 150)  * scale,
            Boss      = rand.Next(0, 80),
            Pk        = rand.Next(0, 25),
            LoginRate = rand.Next(0, 31),
            Chat      = rand.Next(0, 400),
            Enchant   = rand.Next(0, 60),
            Summon    = rand.Next(0, 40),
            Trade     = rand.Next(0, 120),
            SealQuest = rand.Next(0, 100)
        };
    }

    private record ClanDef(
        string Name, string Server, int Level, long Reputation, long Experience,
        int Wins, int Losses, bool HasAjit, bool HasCastle, bool IsAtWar,
        int BloodOathLevel, JoinPolicy JoinPolicy,
        string Introduction, string Notice, string Emblem,
        string[] Rivals, string[] Allies);

    private static ClanDef[] GetClanDefinitions() =>
    [
        new("켄라우헬의용군", "켄라우헬", 10, 985000L, 98_500_000L, 142, 23, true,  true,  false,
            6, JoinPolicy.Approval,
            "서버 최강 혈맹. 공성전 경험 풍부. 레벨 70 이상 전투 특화 혈맹원 우선 모집.",
            "매주 토요일 공성전 필참! 혈맹원 모집 중 (레벨 70 이상)\n직속혈맹/근위대 우선 모집.",
            "⚔",
            ["켄라우헬기사단"], ["용사의동맹"]),

        new("켄라우헬기사단", "케레니스", 8,  712000L, 71_200_000L, 98,  41, true,  false, true,
            4, JoinPolicy.Approval,
            "기사도를 중시하는 정예 혈맹. 현재 켄라우헬의용군과 혈맹전 진행 중.",
            "신규 혈맹원 환영합니다. 현재 켄라우헬의용군과 혈맹전 중.\n기사단 증원 모집 중.",
            "🛡",
            ["켄라우헬의용군"], ["흑의기사단"]),

        new("흑의기사단",     "안타라스", 7,  534000L, 53_400_000L, 77,  55, true,  false, false,
            3, JoinPolicy.Approval,
            "전투 특화 혈맹. 공성전 준비 중. 아카데미 운영.",
            "공성전 준비 중. 전투 특화 혈맹원 우선 모집.\n아카데미 신입 환영 (40레벨 미만).",
            "🗡",
            [], ["켄라우헬기사단"]),

        new("용사의동맹",     "판도라",   5,  321000L, 32_100_000L, 45,  38, false, false, false,
            2, JoinPolicy.Immediate,
            "함께 성장하는 혈맹. 신규 유저 환영. 레벨 10 이상 즉시 가입 가능.",
            "함께 성장하는 혈맹입니다. 레벨 10 이상 가입 가능.\n아카데미 운영 중 - 신입 대환영!",
            "🌟",
            [], ["켄라우헬의용군"]),

        new("붉은달",         "발라카스", 4,  198000L, 19_800_000L, 29,  30, false, false, false,
            1, JoinPolicy.Password,
            "야간 사냥 위주의 소규모 정예 혈맹. 오전 2시~6시 주 활동.",
            "야간 사냥 위주 활동. 오전 2시~6시 주 활동 시간.\n소규모 정예 혈맹, 정예 이상 승격 가능.",
            "🌙",
            [], []),
    ];
}
