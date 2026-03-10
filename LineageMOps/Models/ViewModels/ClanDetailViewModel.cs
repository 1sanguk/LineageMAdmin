using LineageMOps.Models.Domain;

namespace LineageMOps.Models.ViewModels;

public class ClanDetailViewModel
{
    public Clan Clan { get; set; } = null!;
    public List<ClanMember> MainMembers { get; set; } = new();
    public List<ClanMember> AcademyMembers { get; set; } = new();
    public IReadOnlyDictionary<ClanRank, List<ClanMember>> ByRank { get; set; }
        = new Dictionary<ClanRank, List<ClanMember>>();
}
