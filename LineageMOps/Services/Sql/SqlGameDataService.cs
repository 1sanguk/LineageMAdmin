using LineageMOps.Data;
using LineageMOps.Models.Domain;
using LineageMOps.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LineageMOps.Services.Sql;

public class SqlGameDataService : IGameDataService
{
    private readonly LineageMOpsDbContext _db;

    public SqlGameDataService(LineageMOpsDbContext db) => _db = db;

    public PaginatedList<Character> SearchCharacters(string? query, string? server, string? cls, int page, int pageSize)
    {
        var characters = _db.Characters.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
            characters = characters.Where(c => c.Name.Contains(query));

        if (!string.IsNullOrWhiteSpace(server))
            characters = characters.Where(c => c.Server == server);

        if (!string.IsNullOrWhiteSpace(cls) && Enum.TryParse<CharacterClass>(cls, out var parsedClass))
            characters = characters.Where(c => c.Class == parsedClass);

        var totalCount = characters.Count();
        var items = characters.OrderByDescending(c => c.Level)
                              .Skip((page - 1) * pageSize)
                              .Take(pageSize)
                              .ToList();
        return PaginatedList<Character>.From(items, totalCount, page, pageSize);
    }

    public Character? GetCharacter(int id) =>
        _db.Characters.Include(c => c.Inventory).FirstOrDefault(c => c.Id == id);

    public List<Character> GetCharactersByAccountId(int accountId) =>
        _db.Characters.Where(c => c.AccountId == accountId).ToList();

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

    public void UpdateLevelExp(int characterId, int level, long experience)
    {
        var character = _db.Characters.Find(characterId);
        if (character == null) return;
        level = Math.Clamp(level, 1, LevelTable.MaxLevel);
        character.Level = level;
        character.Experience = Math.Max(0, experience);
        character.MaxExperience = LevelTable.GetMaxXP(level);
        _db.SaveChanges();
    }

    public void UpdateStats(int characterId, CharacterStats stats)
    {
        var character = _db.Characters.Find(characterId);
        if (character == null) return;
        character.Stats.Str = stats.Str;
        character.Stats.Dex = stats.Dex;
        character.Stats.Con = stats.Con;
        character.Stats.Wis = stats.Wis;
        character.Stats.Int = stats.Int;
        character.Stats.Cha = stats.Cha;
        character.Stats.Hp = stats.Hp;
        character.Stats.MaxHp = stats.MaxHp;
        character.Stats.Mp = stats.Mp;
        character.Stats.MaxMp = stats.MaxMp;
        character.Stats.Ac = stats.Ac;
        character.Stats.Lfe = stats.Lfe;
        character.Stats.Dth = stats.Dth;
        _db.SaveChanges();
    }

    public void UpdateCurrency(int characterId, int adena, int diamond)
    {
        var character = _db.Characters.Find(characterId);
        if (character == null) return;
        character.Adena = adena;
        character.Diamond = diamond;
        _db.SaveChanges();
    }

    public List<Item> GetAllItems() =>
        _db.Items.OrderBy(i => i.Type).ThenBy(i => i.Name).ToList();

    public void AddInventoryItem(int characterId, string itemName, ItemGrade grade, int quantity, int enchant, bool isEquipped)
    {
        _db.InventoryItems.Add(new InventoryItem
        {
            CharacterId = characterId,
            ItemName = itemName,
            Grade = grade,
            Quantity = quantity,
            Enchant = enchant,
            IsEquipped = isEquipped
        });
        _db.SaveChanges();
    }

    public void AddAllItems(int characterId)
    {
        foreach (var item in _db.Items.ToList())
        {
            _db.InventoryItems.Add(new InventoryItem
            {
                CharacterId = characterId,
                ItemId = item.Id,
                ItemName = item.Name,
                Grade = item.Grade,
                Quantity = 1,
                Enchant = 0,
                IsEquipped = false
            });
        }
        _db.SaveChanges();
    }

    public void UpdateInventoryItem(int characterId, int inventoryItemId, int quantity, int enchant, bool isEquipped)
    {
        var item = _db.InventoryItems.FirstOrDefault(i => i.Id == inventoryItemId && i.CharacterId == characterId);
        if (item == null) return;
        item.Quantity = quantity;
        item.Enchant = enchant;
        item.IsEquipped = isEquipped;
        _db.SaveChanges();
    }

    public void RemoveInventoryItem(int characterId, int inventoryItemId)
    {
        var item = _db.InventoryItems.FirstOrDefault(i => i.Id == inventoryItemId && i.CharacterId == characterId);
        if (item == null) return;
        _db.InventoryItems.Remove(item);
        _db.SaveChanges();
    }
}
