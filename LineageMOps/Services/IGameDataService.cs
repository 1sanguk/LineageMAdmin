using LineageMOps.Models.Domain;
using LineageMOps.Models.ViewModels;

namespace LineageMOps.Services;

public interface IGameDataService
{
    PaginatedList<Character> SearchCharacters(string? query, string? server, string? cls, int page, int pageSize);
    Character? GetCharacter(int id);
    List<Character> GetCharactersByAccountId(int accountId);
    List<string> BuildStatChanges(CharacterStats old, CharacterStats updated);
    void UpdateStats(int characterId, CharacterStats stats);
    void UpdateLevelExp(int characterId, int level, long experience);
    void UpdateCurrency(int characterId, int adena, int diamond);
    List<Item> GetAllItems();
    void AddInventoryItem(int characterId, string itemName, ItemGrade grade, int quantity, int enchant, bool isEquipped);
    void AddAllItems(int characterId);
    void UpdateInventoryItem(int characterId, int inventoryItemId, int quantity, int enchant, bool isEquipped);
    void RemoveInventoryItem(int characterId, int inventoryItemId);
}
