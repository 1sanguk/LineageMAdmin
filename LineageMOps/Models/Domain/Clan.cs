using LineageMOps.Constants;

namespace LineageMOps.Models.Domain;

public class Clan
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Server { get; set; } = "";

    /// <summary>혈맹 레벨 (1~10)</summary>
    public int Level { get; set; }

    /// <summary>혈맹 명성치</summary>
    public long Reputation { get; set; }

    /// <summary>혈맹 경험치</summary>
    public long Experience { get; set; }

    public int LeaderCharacterId { get; set; }
    public string LeaderName { get; set; } = "";

    private List<ClanMember> _members = new();
    public List<ClanMember> Members
    {
        get => _members;
        set => _members = value ?? new();
    }

    public int MemberCount  => _members.Count(m => m.Rank != ClanRank.Academy);
    public int AcademyCount => _members.Count(m => m.Rank == ClanRank.Academy);

    public int MaxMembers => Math.Min(
        Level * ClanConstants.MembersPerLevel + BloodOathLevel * ClanConstants.BloodOathMembersPerLevel,
        ClanConstants.MaxMemberCapacity);
    public int MaxAcademy => Level >= ClanConstants.AcademyMinLevel ? ClanConstants.AcademyMaxCapacity : 0;
    public bool HasAcademy => Level >= ClanConstants.AcademyMinLevel;

    /// <summary>피의 서약 레벨 (0=미개방, 1~6)</summary>
    public int BloodOathLevel { get; set; }

    /// <summary>가입 정책</summary>
    public JoinPolicy JoinPolicy { get; set; }

    public bool HasAjit { get; set; }
    public bool HasCastle { get; set; }
    public bool IsAtWar { get; set; }

    public int WarWins { get; set; }
    public int WarLosses { get; set; }
    public int WinRate
    {
        get
        {
            var total = WarWins + WarLosses;
            return total > 0 ? (int)((double)WarWins / total * 100) : 0;
        }
    }

    /// <summary>혈맹 소개글</summary>
    public string Introduction { get; set; } = "";

    /// <summary>혈맹 공지사항</summary>
    public string Notice { get; set; } = "";

    public string Emblem { get; set; } = "";

    /// <summary>적대 혈맹 이름 목록</summary>
    public List<string> RivalClanNames { get; set; } = new();

    /// <summary>동맹 혈맹 이름 목록</summary>
    public List<string> AllyClanNames { get; set; } = new();

    public bool IsDisbanded { get; set; }
    public DateTime CreatedAt { get; set; }
}
