using LineageMOps.Models.Domain;

namespace LineageMOps.Services;

public interface IGameDataService
{
    List<Character> SearchCharacters(string? query, string? server, string? cls, int page, int pageSize, out int totalCount);
    Character? GetCharacter(int id);
    void UpdateStats(int characterId, CharacterStats stats);
    void UpdateLevelExp(int characterId, int level, long experience);
    void UpdateCurrency(int characterId, int adena, int diamond);
    List<Item> GetAllItems();
    void AddInventoryItem(int characterId, string itemName, ItemGrade grade, int quantity, int enchant, bool isEquipped);
    void AddAllItems(int characterId);
    void UpdateInventoryItem(int characterId, int inventoryItemId, int quantity, int enchant, bool isEquipped);
    void RemoveInventoryItem(int characterId, int inventoryItemId);
}
