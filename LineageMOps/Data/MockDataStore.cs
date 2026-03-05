using LineageMOps.Models.Domain;

namespace LineageMOps.Data;

public class MockDataStore
{
    // Static arrays must be declared before _instance to avoid null during static init
    private static readonly string[] ServerNames = { "켄라우헬", "바츠", "기란", "오렌", "아덴", "글루디오", "디온" };
    private static readonly string[] Classes = { "군주", "기사", "요정", "마법사", "다크엘프", "드래곤나이트", "환술사" };

    private static readonly MockDataStore _instance = new();
    public static MockDataStore Instance => _instance;

    public List<Account> Accounts { get; private set; }
    public List<Character> Characters { get; private set; }
    public List<Item> Items { get; private set; }
    public List<GameEvent> Events { get; private set; }
    public List<Notice> Notices { get; private set; }
    public List<ServerLog> Logs { get; private set; }
    public List<ServerStatus> Servers { get; private set; }
    public List<AdminLog> AdminLogs { get; private set; } = new();

    private MockDataStore()
    {
        Servers = SeedServers();
        Accounts = SeedAccounts();
        Characters = SeedCharacters();
        Items = SeedItems();
        Events = SeedEvents();
        Notices = SeedNotices();
        Logs = SeedLogs();
    }

    private List<ServerStatus> SeedServers()
    {
        return new List<ServerStatus>
        {
            new() { Name = "kenrauhel", DisplayName = "켄라우헬", State = ServerState.Online, CurrentPlayers = 4821, MaxPlayers = 8000, CpuUsage = 42.3, MemoryUsage = 61.7, Uptime = 99.98 },
            new() { Name = "bartz", DisplayName = "바츠", State = ServerState.Online, CurrentPlayers = 6103, MaxPlayers = 8000, CpuUsage = 58.1, MemoryUsage = 73.2, Uptime = 99.95 },
            new() { Name = "giran", DisplayName = "기란", State = ServerState.Warning, CurrentPlayers = 7854, MaxPlayers = 8000, CpuUsage = 89.4, MemoryUsage = 88.6, Uptime = 99.87 },
            new() { Name = "oren", DisplayName = "오렌", State = ServerState.Online, CurrentPlayers = 3241, MaxPlayers = 8000, CpuUsage = 31.2, MemoryUsage = 48.9, Uptime = 100.0 },
            new() { Name = "aden", DisplayName = "아덴", State = ServerState.Maintenance, CurrentPlayers = 0, MaxPlayers = 8000, CpuUsage = 5.1, MemoryUsage = 22.3, Uptime = 98.21 },
            new() { Name = "gludio", DisplayName = "글루디오", State = ServerState.Online, CurrentPlayers = 5512, MaxPlayers = 8000, CpuUsage = 52.8, MemoryUsage = 67.4, Uptime = 99.99 },
            new() { Name = "dion", DisplayName = "디온", State = ServerState.Online, CurrentPlayers = 4098, MaxPlayers = 8000, CpuUsage = 45.6, MemoryUsage = 59.1, Uptime = 99.96 },
        };
    }

    private List<Account> SeedAccounts()
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
                Server = ServerNames[rand.Next(ServerNames.Length)],
                RegisteredAt = regDate,
                LastLoginAt = regDate.AddDays(rand.Next(1, (int)(DateTime.Now - regDate).TotalDays)),
                IpAddress = $"192.168.{rand.Next(1, 255)}.{rand.Next(1, 255)}",
                Status = status,
                Sanctions = new List<BannedRecord>()
            };

            if (status == AccountStatus.Suspended || status == AccountStatus.Banned || rand.Next(10) < 2)
            {
                var sanctionType = status == AccountStatus.Banned ? SanctionType.PermanentBan :
                                   rand.Next(2) == 0 ? SanctionType.ChatBan : SanctionType.LoginRestriction;
                var startDate = account.RegisteredAt.AddDays(rand.Next(1, 100));
                account.Sanctions.Add(new BannedRecord
                {
                    Id = i * 10,
                    AccountId = i,
                    Type = sanctionType,
                    Reason = GetSanctionReason(sanctionType, rand),
                    StartDate = startDate,
                    EndDate = sanctionType == SanctionType.PermanentBan ? null : startDate.AddDays(rand.Next(1, 30)),
                    OperatorId = $"op_{rand.Next(1, 5):D3}",
                    IsActive = status != AccountStatus.Active
                });
            }

            accounts.Add(account);
        }
        return accounts;
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
        var clanNames = new[] { "바츠의용군", "켄라우헬기사단", "흑의기사단", "용사의동맹", "붉은달", "", "", "" };
        int id = 1;

        foreach (var account in Accounts)
        {
            int charCount = rand.Next(1, 4);
            for (int c = 0; c < charCount; c++)
            {
                var cls = charClasses[rand.Next(charClasses.Length)];
                var level = rand.Next(1, 120);
                var maxExp = (long)level * level * 1000 + 500000;
                var charName = GenerateCharName(rand);
                var ch = new Character
                {
                    Id = id++,
                    AccountId = account.Id,
                    Name = charName,
                    Server = account.Server,
                    Class = cls,
                    Level = level,
                    Experience = rand.NextInt64(0, maxExp),
                    MaxExperience = maxExp,
                    ClanName = clanNames[rand.Next(clanNames.Length)],
                    Adena = rand.Next(10000, 50000000),
                    Diamond = rand.Next(0, 3000),
                    CreatedAt = account.RegisteredAt.AddDays(rand.Next(0, 10)),
                    LastPlayedAt = account.LastLoginAt,
                    Stats = GenerateStats(cls, level, rand),
                    Inventory = new List<InventoryItem>()
                };
                ch.Inventory = GenerateInventory(ch.Id, rand);
                chars.Add(ch);
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
            Str = rand.Next(8, 18) + (cls is CharacterClass.Knight or CharacterClass.DarkKnight ? 3 : 0),
            Dex = rand.Next(8, 18),
            Con = rand.Next(8, 18) + (cls is CharacterClass.Knight ? 2 : 0),
            Wis = rand.Next(8, 18) + (cls is CharacterClass.Wizard or CharacterClass.Elf ? 3 : 0),
            Int = rand.Next(8, 18) + (cls is CharacterClass.Wizard ? 4 : 0),
            Cha = rand.Next(8, 18),
            Hp = (int)(baseHp * 0.7),
            MaxHp = baseHp,
            Mp = (int)(baseMp * 0.8),
            MaxMp = baseMp,
            Ac = rand.Next(-30, 10),
            Lfe = rand.Next(0, 8),
            Dth = rand.Next(0, 5)
        };
    }

    private static List<InventoryItem> GenerateInventory(int charId, Random rand)
    {
        var items = new List<InventoryItem>();
        var weaponNames = new[] { "엘더 블레이드", "그림자의 단검", "마법사의 지팡이", "드워프 도끼", "신성한 활", "고룡의 검" };
        var armorNames = new[] { "강철 갑옷", "그림자 로브", "요정 갑옷", "용린 방어구", "흑철 갑옷" };
        var consumables = new[] { "회복 포션", "마나 포션", "비약", "스크롤", "귀환 주문서" };

        // Weapon
        items.Add(new InventoryItem
        {
            Id = charId * 100 + 1,
            CharacterId = charId,
            ItemId = rand.Next(1, 20),
            ItemName = weaponNames[rand.Next(weaponNames.Length)],
            Grade = (ItemGrade)rand.Next(0, 5),
            Quantity = 1,
            IsEquipped = true,
            Enchant = rand.Next(0, 12)
        });

        // Armor
        items.Add(new InventoryItem
        {
            Id = charId * 100 + 2,
            CharacterId = charId,
            ItemId = rand.Next(20, 40),
            ItemName = armorNames[rand.Next(armorNames.Length)],
            Grade = (ItemGrade)rand.Next(0, 4),
            Quantity = 1,
            IsEquipped = true,
            Enchant = rand.Next(0, 8)
        });

        // Consumables
        int consumableCount = rand.Next(2, 6);
        for (int i = 0; i < consumableCount; i++)
        {
            items.Add(new InventoryItem
            {
                Id = charId * 100 + 3 + i,
                CharacterId = charId,
                ItemId = rand.Next(50, 70),
                ItemName = consumables[rand.Next(consumables.Length)],
                Grade = ItemGrade.Normal,
                Quantity = rand.Next(1, 100),
                IsEquipped = false,
                Enchant = 0
            });
        }

        return items;
    }

    private List<Item> SeedItems()
    {
        return new List<Item>
        {
            new() { Id = 1, Name = "엘더 블레이드", Type = ItemType.Weapon, Grade = ItemGrade.Epic, Description = "고대 기사의 검", Weight = 1540, BuyPrice = 5000000, SellPrice = 1250000, IsTradeble = true },
            new() { Id = 2, Name = "그림자의 단검", Type = ItemType.Weapon, Grade = ItemGrade.Rare, Description = "다크엘프 암살자의 단검", Weight = 520, BuyPrice = 1200000, SellPrice = 300000, IsTradeble = true },
            new() { Id = 3, Name = "마법사의 지팡이", Type = ItemType.Weapon, Grade = ItemGrade.Rare, Description = "마력이 깃든 지팡이", Weight = 420, BuyPrice = 900000, SellPrice = 225000, IsTradeble = true },
            new() { Id = 4, Name = "강철 갑옷", Type = ItemType.Armor, Grade = ItemGrade.Magic, Description = "견고한 강철 갑옷", Weight = 3200, BuyPrice = 800000, SellPrice = 200000, IsTradeble = true },
            new() { Id = 5, Name = "고룡의 검", Type = ItemType.Weapon, Grade = ItemGrade.Legendary, Description = "전설의 용 사냥꾼이 사용했던 검", Weight = 1800, BuyPrice = 50000000, SellPrice = 12500000, IsTradeble = false },
            new() { Id = 6, Name = "회복 포션", Type = ItemType.Consumable, Grade = ItemGrade.Normal, Description = "HP를 500 회복", Weight = 50, BuyPrice = 500, SellPrice = 50, IsTradeble = true },
            new() { Id = 7, Name = "마나 포션", Type = ItemType.Consumable, Grade = ItemGrade.Normal, Description = "MP를 300 회복", Weight = 50, BuyPrice = 800, SellPrice = 80, IsTradeble = true },
            new() { Id = 8, Name = "귀환 주문서", Type = ItemType.Consumable, Grade = ItemGrade.Normal, Description = "마을로 귀환", Weight = 20, BuyPrice = 1000, SellPrice = 100, IsTradeble = true },
        };
    }

    private List<GameEvent> SeedEvents()
    {
        var now = DateTime.Now;
        return new List<GameEvent>
        {
            new() { Id = 1, Title = "봄맞이 경험치 2배 이벤트", Description = "봄을 맞이하여 모든 서버에서 경험치 2배 이벤트를 진행합니다. 이 기간 동안 획득하는 모든 경험치가 2배로 적용됩니다.", Type = EventType.Exp, Status = EventStatus.Active, StartDate = now.AddDays(-5), EndDate = now.AddDays(9), ApplicableServers = ServerNames.ToList(), CreatedBy = "op_001", CreatedAt = now.AddDays(-10) },
            new() { Id = 2, Title = "드랍률 UP! 보물 사냥 이벤트", Description = "일주일간 아이템 드랍률이 1.5배로 증가합니다. 레어 아이템을 노려보세요!", Type = EventType.Drop, Status = EventStatus.Active, StartDate = now.AddDays(-2), EndDate = now.AddDays(5), ApplicableServers = new List<string> { "바츠", "기란", "아덴" }, CreatedBy = "op_002", CreatedAt = now.AddDays(-7) },
            new() { Id = 3, Title = "주간 로그인 보상 이벤트", Description = "매일 접속 시 특별 보상 아이템을 지급합니다. 7일 연속 접속 시 추가 보상!", Type = EventType.Login, Status = EventStatus.Scheduled, StartDate = now.AddDays(3), EndDate = now.AddDays(17), ApplicableServers = ServerNames.ToList(), CreatedBy = "op_001", CreatedAt = now.AddDays(-1) },
            new() { Id = 4, Title = "신년 특별 이벤트", Description = "새해를 맞이하여 진행된 특별 이벤트입니다.", Type = EventType.Holiday, Status = EventStatus.Ended, StartDate = now.AddDays(-60), EndDate = now.AddDays(-46), ApplicableServers = ServerNames.ToList(), CreatedBy = "op_003", CreatedAt = now.AddDays(-65) },
            new() { Id = 5, Title = "혈맹 토너먼트", Description = "서버 최강 혈맹을 가리는 토너먼트입니다. 우승 혈맹에게는 특별 칭호와 보상을 지급합니다.", Type = EventType.Special, Status = EventStatus.Scheduled, StartDate = now.AddDays(14), EndDate = now.AddDays(21), ApplicableServers = new List<string> { "바츠", "기란" }, CreatedBy = "op_001", CreatedAt = now },
        };
    }

    private List<Notice> SeedNotices()
    {
        var now = DateTime.Now;
        return new List<Notice>
        {
            new() { Id = 1, Title = "[긴급] 기란 서버 긴급 점검 안내", Content = "기란 서버에서 일부 유저들이 접속 불안정을 겪고 있어 긴급 점검을 진행합니다.\n\n■ 점검 일시: 2026-03-04 02:00 ~ 04:00 (2시간)\n■ 점검 서버: 기란\n■ 점검 내용: 서버 과부하 원인 조사 및 안정화 작업\n\n점검 중 접속 불가 및 진행 중인 전투가 종료될 수 있습니다.", Type = NoticeType.Maintenance, IsPublished = true, PublishDate = now.AddHours(-2), ApplicableServers = new List<string> { "기란" }, CreatedBy = "op_001", CreatedAt = now.AddHours(-3) },
            new() { Id = 2, Title = "[업데이트] 3월 정기 업데이트 안내", Content = "2026년 3월 정기 업데이트 내용을 안내드립니다.\n\n■ 신규 사냥터: 공허의 탑 9~12층 추가\n■ 신규 아이템: 전설 등급 무기 3종 추가\n■ 밸런스 패치: 군주 직업 스킬 조정\n■ 버그 수정: 특정 상황에서 아이템이 사라지는 문제 수정", Type = NoticeType.Update, IsPublished = true, PublishDate = now.AddDays(-1), ApplicableServers = ServerNames.ToList(), CreatedBy = "op_002", CreatedAt = now.AddDays(-2) },
            new() { Id = 3, Title = "[이벤트] 봄맞이 경험치 2배 이벤트 안내", Content = "봄을 맞이하여 경험치 2배 이벤트를 진행합니다!\n\n■ 이벤트 기간: 2026-02-28 ~ 2026-03-13\n■ 적용 서버: 전 서버\n■ 이벤트 내용: 모든 경험치 2배 획득", Type = NoticeType.Event, IsPublished = true, PublishDate = now.AddDays(-5), ApplicableServers = ServerNames.ToList(), CreatedBy = "op_001", CreatedAt = now.AddDays(-6) },
            new() { Id = 4, Title = "[공지] 불법 프로그램 사용 제재 강화 안내", Content = "최근 불법 자동 사냥 프로그램 사용자가 증가함에 따라 제재를 강화합니다.\n\n■ 1차 적발: 7일 접속 제한\n■ 2차 적발: 30일 접속 제한\n■ 3차 적발: 영구 정지\n\n공정한 게임 환경을 위해 불법 프로그램 사용을 자제해 주시기 바랍니다.", Type = NoticeType.General, IsPublished = true, PublishDate = now.AddDays(-7), ApplicableServers = ServerNames.ToList(), CreatedBy = "op_003", CreatedAt = now.AddDays(-8) },
            new() { Id = 5, Title = "[예정] 4월 대규모 업데이트 예고", Content = "4월에 예정된 대규모 업데이트를 예고합니다. 상세 내용은 추후 공지 예정입니다.", Type = NoticeType.Update, IsPublished = false, PublishDate = now.AddDays(10), ApplicableServers = ServerNames.ToList(), CreatedBy = "op_001", CreatedAt = now },
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

        int id = 1;
        for (int i = 0; i < 200; i++)
        {
            var ts = now.AddMinutes(-rand.Next(0, 1440));
            var account = loginUsers[rand.Next(loginUsers.Count)];
            var logType = (LogType)rand.Next(0, 7);

            logs.Add(new ServerLog
            {
                Id = id++,
                Server = ServerNames[rand.Next(ServerNames.Length)],
                Type = logType,
                Level = logType == LogType.Error ? Models.Domain.LogLevel.Error : logType == LogType.System ? Models.Domain.LogLevel.Warning : Models.Domain.LogLevel.Info,
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
                Timestamp = ts,
                IpAddress = logType is LogType.Login ? account.IpAddress : null,
                Detail = logType == LogType.Error ? $"ErrorCode: E{rand.Next(1000, 9999)}" : null
            });
        }

        logs = logs.OrderByDescending(l => l.Timestamp).ToList();
        return logs;
    }

    // Mutating helpers
    public void AddSanction(BannedRecord sanction)
    {
        var account = Accounts.FirstOrDefault(a => a.Id == sanction.AccountId);
        if (account == null) return;
        sanction.Id = Accounts.SelectMany(a => a.Sanctions).Any() ? Accounts.SelectMany(a => a.Sanctions).Max(s => s.Id) + 1 : 1;
        account.Sanctions.Add(sanction);
        account.Status = sanction.Type == SanctionType.PermanentBan ? AccountStatus.Banned : AccountStatus.Suspended;
    }

    public void SaveEvent(GameEvent evt)
    {
        var existing = Events.FirstOrDefault(e => e.Id == evt.Id);
        if (existing != null)
        {
            Events.Remove(existing);
        }
        else
        {
            evt.Id = Events.Any() ? Events.Max(e => e.Id) + 1 : 1;
        }
        Events.Add(evt);
    }

    public void DeleteEvent(int id) => Events.RemoveAll(e => e.Id == id);

    public void SaveNotice(Notice notice)
    {
        var existing = Notices.FirstOrDefault(n => n.Id == notice.Id);
        if (existing != null)
        {
            Notices.Remove(existing);
        }
        else
        {
            notice.Id = Notices.Any() ? Notices.Max(n => n.Id) + 1 : 1;
        }
        Notices.Add(notice);
    }

    public void DeleteNotice(int id) => Notices.RemoveAll(n => n.Id == id);
}
