using LineageMOps.Data;
using LineageMOps.Models.Domain;
using LineageMOps.Models.ViewModels;

namespace LineageMOps.Services;

public class GameDataService : IGameDataService
{
    private readonly MockDataStore _store;

    public GameDataService(MockDataStore store) => _store = store;

    public PaginatedList<Character> SearchCharacters(string? query, string? server, string? cls, int page, int pageSize)
    {
        var characters = _store.Characters.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(query))
            characters = characters.Where(c => c.Name.Contains(query, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(server))
            characters = characters.Where(c => c.Server == server);

        if (!string.IsNullOrWhiteSpace(cls) && Enum.TryParse<CharacterClass>(cls, out var parsedClass))
            characters = characters.Where(c => c.Class == parsedClass);

        var sorted = characters.OrderByDescending(c => c.Level).ToList();
        var items = sorted.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return PaginatedList<Character>.From(items, sorted.Count, page, pageSize);
    }

    public Character? GetCharacter(int id) => _store.Characters.FirstOrDefault(c => c.Id == id);

    public List<Character> GetCharactersByAccountId(int accountId) =>
        _store.Characters.Where(c => c.AccountId == accountId).ToList();

    public List<string> BuildStatChanges(CharacterStats old, CharacterStats updated)
    {
        var changes = new List<string>();
        if (old.Str != updated.Str) changes.Add($"STR:{old.Str}→{updated.Str}");
        if (old.Dex != updated.Dex) changes.Add($"DEX:{old.Dex}→{updated.Dex}");
        if (old.Con != updated.Con) changes.Add($"CON:{old.Con}→{updated.Con}");
        if (old.Wis != updated.Wis) changes.Add($"WIS:{old.Wis}→{updated.Wis}");
        if (old.Int != updated.Int) changes.Add($"INT:{old.Int}→{updated.Int}");
        if (old.Cha != updated.Cha) changes.Add($"CHA:{old.Cha}→{updated.Cha}");
        if (old.Hp != updated.Hp) changes.Add($"HP:{old.Hp}→{updated.Hp}");
        if (old.MaxHp != updated.MaxHp) changes.Add($"MaxHP:{old.MaxHp}→{updated.MaxHp}");
        if (old.Mp != updated.Mp) changes.Add($"MP:{old.Mp}→{updated.Mp}");
        if (old.MaxMp != updated.MaxMp) changes.Add($"MaxMP:{old.MaxMp}→{updated.MaxMp}");
        if (old.Ac != updated.Ac) changes.Add($"AC:{old.Ac}→{updated.Ac}");
        if (old.Lfe != updated.Lfe) changes.Add($"LFE:{old.Lfe}→{updated.Lfe}");
        if (old.Dth != updated.Dth) changes.Add($"DTH:{old.Dth}→{updated.Dth}");
        return changes;
    }

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
        character.Inventory.Add(new InventoryItem
        {
            Id = NextInventoryId(),
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
        var baseId = NextInventoryId();
        var index = 0;
        foreach (var item in _store.Items)
        {
            character.Inventory.Add(new InventoryItem
            {
                Id = baseId + index++,
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

    private int NextInventoryId() =>
        _store.Characters.SelectMany(c => c.Inventory).Select(i => i.Id).DefaultIfEmpty(0).Max() + 1;
}
