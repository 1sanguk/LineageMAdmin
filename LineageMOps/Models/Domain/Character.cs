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

public enum CharacterClass
{
    Knight,
    DarkKnight,
    Elf,
    Wizard,
    DarkElf,
    Dragonknight,
    Illusionist
}

public class CharacterStats
{
    public int Str { get; set; }
    public int Dex { get; set; }
    public int Con { get; set; }
    public int Wis { get; set; }
    public int Int { get; set; }
    public int Cha { get; set; }
    public int Hp { get; set; }
    public int MaxHp { get; set; }
    public int Mp { get; set; }
    public int MaxMp { get; set; }
    public int Ac { get; set; }
    public int Lfe { get; set; }
    public int Dth { get; set; }
}

public class InventoryItem
{
    public int Id { get; set; }
    public int CharacterId { get; set; }
    public int ItemId { get; set; }
    public string ItemName { get; set; } = "";
    public ItemGrade Grade { get; set; }
    public int Quantity { get; set; }
    public bool IsEquipped { get; set; }
    public int Enchant { get; set; }
}

public enum ItemGrade
{
    Normal,
    Magic,
    Rare,
    Epic,
    Legendary
}
