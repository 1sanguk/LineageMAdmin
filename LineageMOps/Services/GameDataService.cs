using LineageMOps.Data;
using LineageMOps.Models.Domain;

namespace LineageMOps.Services;

public class GameDataService : IGameDataService
{
    private readonly MockDataStore _store;

    public GameDataService(MockDataStore store) => _store = store;

    public List<Character> SearchCharacters(string? query, string? server, string? cls, int page, int pageSize, out int totalCount)
    {
        var q = _store.Characters.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(query))
            q = q.Where(c => c.Name.Contains(query, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(server))
            q = q.Where(c => c.Server == server);

        if (!string.IsNullOrWhiteSpace(cls) && Enum.TryParse<CharacterClass>(cls, out var parsedClass))
            q = q.Where(c => c.Class == parsedClass);

        var list = q.OrderByDescending(c => c.Level).ToList();
        totalCount = list.Count;
        return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }

    public Character? GetCharacter(int id) => _store.Characters.FirstOrDefault(c => c.Id == id);

    public void UpdateStats(int characterId, CharacterStats stats)
    {
        var character = _store.Characters.FirstOrDefault(c => c.Id == characterId);
        if (character == null) return;
        character.Stats = stats;
    }

    public void UpdateLevelExp(int characterId, int level, long experience)
    {
        var character = _store.Characters.FirstOrDefault(c => c.Id == characterId);
        if (character == null) return;
        level = Math.Clamp(level, 1, LevelTable.MaxLevel);
        character.Level = level;
        character.Experience = Math.Max(0, experience);
        character.MaxExperience = LevelTable.GetMaxXP(level);
    }

    public void UpdateCurrency(int characterId, int adena, int diamond)
    {
        var character = _store.Characters.FirstOrDefault(c => c.Id == characterId);
        if (character == null) return;
        character.Adena = adena;
        character.Diamond = diamond;
    }

    public List<Item> GetAllItems() => _store.Items;

    public void AddInventoryItem(int characterId, string itemName, ItemGrade grade, int quantity, int enchant, bool isEquipped)
    {
        var character = _store.Characters.FirstOrDefault(c => c.Id == characterId);
        if (character == null) return;
        var newId = _store.Characters.SelectMany(c => c.Inventory).Select(i => i.Id).DefaultIfEmpty(0).Max() + 1;
        character.Inventory.Add(new InventoryItem
        {
            Id = newId,
            CharacterId = characterId,
            ItemName = itemName,
            Grade = grade,
            Quantity = quantity,
            Enchant = enchant,
            IsEquipped = isEquipped
        });
    }

    public void AddAllItems(int characterId)
    {
        var character = _store.Characters.FirstOrDefault(c => c.Id == characterId);
        if (character == null) return;
        var baseId = _store.Characters.SelectMany(c => c.Inventory).Select(i => i.Id).DefaultIfEmpty(0).Max() + 1;
        int idx = 0;
        foreach (var item in _store.Items)
        {
            character.Inventory.Add(new InventoryItem
            {
                Id = baseId + idx++,
                CharacterId = characterId,
                ItemId = item.Id,
                ItemName = item.Name,
                Grade = item.Grade,
                Quantity = 1,
                Enchant = 0,
                IsEquipped = false
            });
        }
    }

    public void UpdateInventoryItem(int characterId, int inventoryItemId, int quantity, int enchant, bool isEquipped)
    {
        var character = _store.Characters.FirstOrDefault(c => c.Id == characterId);
        var item = character?.Inventory.FirstOrDefault(i => i.Id == inventoryItemId);
        if (item == null) return;
        item.Quantity = quantity;
        item.Enchant = enchant;
        item.IsEquipped = isEquipped;
    }

    public void RemoveInventoryItem(int characterId, int inventoryItemId)
    {
        var character = _store.Characters.FirstOrDefault(c => c.Id == characterId);
        if (character == null) return;
        var item = character.Inventory.FirstOrDefault(i => i.Id == inventoryItemId);
        if (item != null) character.Inventory.Remove(item);
    }
}
