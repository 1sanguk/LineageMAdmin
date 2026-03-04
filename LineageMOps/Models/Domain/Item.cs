namespace LineageMOps.Models.Domain;

public class Item
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public ItemType Type { get; set; }
    public ItemGrade Grade { get; set; }
    public string Description { get; set; } = "";
    public int Weight { get; set; }
    public long BuyPrice { get; set; }
    public long SellPrice { get; set; }
    public bool IsTradeble { get; set; }
}

public enum ItemType
{
    Weapon,
    Armor,
    Accessory,
    Consumable,
    Material,
    Quest,
    Etc
}
