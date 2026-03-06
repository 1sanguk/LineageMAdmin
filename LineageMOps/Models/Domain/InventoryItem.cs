namespace LineageMOps.Models.Domain;

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
