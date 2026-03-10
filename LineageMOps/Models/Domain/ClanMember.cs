namespace LineageMOps.Models.Domain;

public class ClanMember
{
    public int CharacterId { get; set; }
    public string CharacterName { get; set; } = "";
    public string ClassName { get; set; } = "";
    public int Level { get; set; }
    public ClanRank Rank { get; set; }
    public DateTime JoinedAt { get; set; }
    public DateTime LastOnlineAt { get; set; }
    public bool IsOnline { get; set; }
    public ClanActivityScore ActivityScore { get; set; } = new();
}
