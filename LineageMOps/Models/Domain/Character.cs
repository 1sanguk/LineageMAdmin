namespace LineageMOps.Models.Domain;

public class Character
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public string Name { get; set; } = "";
    public string Server { get; set; } = "";
    public CharacterClass Class { get; set; }
    public int Level { get; set; }
    public long Experience { get; set; }
    public long MaxExperience { get; set; }
    public string ClanName { get; set; } = "";
    public int Adena { get; set; }
    public int Diamond { get; set; }
    public CharacterStats Stats { get; set; } = new();
    public List<InventoryItem> Inventory { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime LastPlayedAt { get; set; }
}
