using LineageMOps.Data;
using LineageMOps.Models.Domain;
using LineageMOps.Models.ViewModels;

namespace LineageMOps.Services;

public class ClanService : IClanService
{
    private readonly MockDataStore _store;

    public ClanService(MockDataStore store) => _store = store;

    public PaginatedList<Clan> Search(string? query, string? server, int? level, int page, int pageSize)
    {
        var clans = _store.Clans.Where(c => !c.IsDisbanded).AsEnumerable();

        if (!string.IsNullOrWhiteSpace(query))
            clans = clans.Where(c => c.Name.Contains(query, StringComparison.OrdinalIgnoreCase)
                                  || c.LeaderName.Contains(query, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(server))
            clans = clans.Where(c => c.Server == server);

        if (level.HasValue)
            clans = clans.Where(c => c.Level == level.Value);

        var sorted = clans.OrderByDescending(c => c.Reputation).ToList();
        return PaginatedList<Clan>.From(sorted.Skip((page - 1) * pageSize).Take(pageSize).ToList(), sorted.Count, page, pageSize);
    }

    public Clan? GetById(int id) => _store.Clans.FirstOrDefault(c => c.Id == id);

    public ClanDetailViewModel? GetDetail(int id)
    {
        var clan = GetById(id);
        if (clan == null) return null;

        var mainMembers    = clan.Members.Where(m => m.Rank != ClanRank.Academy).ToList();
        var academyMembers = clan.Members.Where(m => m.Rank == ClanRank.Academy).ToList();
        var byRank = mainMembers
            .GroupBy(m => m.Rank)
            .ToDictionary(g => g.Key, g => g.ToList());

        return new ClanDetailViewModel
        {
            Clan           = clan,
            MainMembers    = mainMembers,
            AcademyMembers = academyMembers,
            ByRank         = byRank,
        };
    }

    public bool UpdateNotice(int id, string notice, out Clan? clan)
    {
        clan = GetById(id);
        if (clan == null) return false;
        clan.Notice = notice;
        return true;
    }

    public bool UpdateIntroduction(int id, string introduction, out Clan? clan)
    {
        clan = GetById(id);
        if (clan == null) return false;
        clan.Introduction = introduction;
        return true;
    }

    public bool UpdateBasicInfo(int id, int level, long reputation, long experience,
                                bool hasAjit, bool hasCastle, bool isAtWar,
                                int warWins, int warLosses, out Clan? clan)
    {
        clan = GetById(id);
        if (clan == null) return false;
        clan.Level      = Math.Clamp(level, 1, 10);
        clan.Reputation = Math.Max(0, reputation);
        clan.Experience = Math.Max(0, experience);
        clan.HasAjit    = hasAjit;
        clan.HasCastle  = hasCastle;
        clan.IsAtWar    = isAtWar;
        clan.WarWins    = Math.Max(0, warWins);
        clan.WarLosses  = Math.Max(0, warLosses);
        return true;
    }

    public bool UpdateSettings(int id, JoinPolicy joinPolicy, int bloodOathLevel, out Clan? clan)
    {
        clan = GetById(id);
        if (clan == null) return false;
        clan.JoinPolicy     = joinPolicy;
        clan.BloodOathLevel = Math.Clamp(bloodOathLevel, 0, 6);
        return true;
    }

    public bool AddRival(int clanId, string rivalName)
    {
        var clan = GetById(clanId);
        if (clan == null || string.IsNullOrWhiteSpace(rivalName)) return false;
        rivalName = rivalName.Trim();
        if (!clan.RivalClanNames.Contains(rivalName))
            clan.RivalClanNames.Add(rivalName);
        return true;
    }

    public bool RemoveRival(int clanId, string rivalName)
    {
        var clan = GetById(clanId);
        if (clan == null) return false;
        return clan.RivalClanNames.Remove(rivalName);
    }

    public bool AddAlly(int clanId, string allyName)
    {
        var clan = GetById(clanId);
        if (clan == null || string.IsNullOrWhiteSpace(allyName)) return false;
        allyName = allyName.Trim();
        if (!clan.AllyClanNames.Contains(allyName))
            clan.AllyClanNames.Add(allyName);
        return true;
    }

    public bool RemoveAlly(int clanId, string allyName)
    {
        var clan = GetById(clanId);
        if (clan == null) return false;
        return clan.AllyClanNames.Remove(allyName);
    }

    public bool KickMember(int clanId, int characterId, out string? memberName)
    {
        memberName = null;
        var clan = GetById(clanId);
        if (clan == null) return false;
        var member = clan.Members.FirstOrDefault(m => m.CharacterId == characterId);
        if (member == null || member.Rank == ClanRank.Leader) return false;
        memberName = member.CharacterName;
        clan.Members.Remove(member);
        return true;
    }

    public bool ChangeMemberRank(int clanId, int characterId, ClanRank newRank, out string? memberName)
    {
        memberName = null;
        var clan = GetById(clanId);
        if (clan == null) return false;
        var member = clan.Members.FirstOrDefault(m => m.CharacterId == characterId);
        if (member == null) return false;
        memberName  = member.CharacterName;
        member.Rank = newRank;
        return true;
    }

    public bool Disband(int id, out Clan? clan)
    {
        clan = GetById(id);
        if (clan == null) return false;
        clan.IsDisbanded = true;
        return true;
    }
}
