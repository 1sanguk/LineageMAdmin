using LineageMOps.Data;
using LineageMOps.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace LineageMOps.Services.Sql;

public class SqlGameDataService : IGameDataService
{
    private readonly LineageMOpsDbContext _db;

    public SqlGameDataService(LineageMOpsDbContext db) => _db = db;

    public List<Character> SearchCharacters(string? query, string? server, string? cls, int page, int pageSize, out int totalCount)
    {
        var q = _db.Characters.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
            q = q.Where(c => c.Name.Contains(query));

        if (!string.IsNullOrWhiteSpace(server))
            q = q.Where(c => c.Server == server);

        if (!string.IsNullOrWhiteSpace(cls) && Enum.TryParse<CharacterClass>(cls, out var parsedClass))
            q = q.Where(c => c.Class == parsedClass);

        totalCount = q.Count();
        return q.OrderByDescending(c => c.Level)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
    }

    public Character? GetCharacter(int id) =>
        _db.Characters.Include(c => c.Inventory).FirstOrDefault(c => c.Id == id);

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
        var items = _db.Items.ToList();
        foreach (var item in items)
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
