using LineageMOps.Models.Domain;
using LineageMOps.Models.ViewModels;
using LineageMOps.Services;
using Microsoft.AspNetCore.Mvc;

namespace LineageMOps.Controllers;

public class GameDataController : Controller
{
    private readonly IGameDataService _gameData;
    private readonly IUserService _users;
    private readonly IAdminLogService _adminLog;
    private const int PageSize = 15;

    public GameDataController(IGameDataService gameData, IUserService users, IAdminLogService adminLog)
    {
        _gameData = gameData;
        _users = users;
        _adminLog = adminLog;
    }

    public IActionResult Index(string? q, string? server, string? cls, int page = 1)
    {
        var chars = _gameData.SearchCharacters(q, server, cls, page, PageSize, out int totalCount);
        ViewBag.Query = q;
        ViewBag.Server = server;
        ViewBag.Class = cls;
        var paged = new PaginatedList<Character>
        {
            Items = chars,
            PageIndex = page,
            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize),
            TotalCount = totalCount,
            PageSize = PageSize
        };
        return View(paged);
    }

    public IActionResult Detail(int id)
    {
        var character = _gameData.GetCharacter(id);
        if (character == null) return NotFound();
        var account = _users.GetById(character.AccountId);
        ViewBag.Account = account;
        ViewBag.AllItems = _gameData.GetAllItems();
        return View(character);
    }

    [HttpPost]
    public IActionResult UpdateLevelExp(int id, int level, long experience)
    {
        var character = _gameData.GetCharacter(id);
        if (character == null) return NotFound();

        var changes = new List<string>();
        if (character.Level != level) changes.Add($"레벨:{character.Level}→{level}");
        if (character.Experience != experience) changes.Add($"경험치:{character.Experience:N0}→{experience:N0}");
        var detail = changes.Any() ? string.Join(", ", changes) : null;

        _gameData.UpdateLevelExp(id, level, experience);
        _adminLog.Add("레벨/경험치 수정", $"{character.Name} (ID:{id})", detail);
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    public IActionResult UpdateStats(int id, CharacterStats stats)
    {
        var character = _gameData.GetCharacter(id);
        if (character == null) return NotFound();

        var old = character.Stats;
        var changes = new List<string>();
        if (old.Str != stats.Str) changes.Add($"STR:{old.Str}→{stats.Str}");
        if (old.Dex != stats.Dex) changes.Add($"DEX:{old.Dex}→{stats.Dex}");
        if (old.Con != stats.Con) changes.Add($"CON:{old.Con}→{stats.Con}");
        if (old.Wis != stats.Wis) changes.Add($"WIS:{old.Wis}→{stats.Wis}");
        if (old.Int != stats.Int) changes.Add($"INT:{old.Int}→{stats.Int}");
        if (old.Cha != stats.Cha) changes.Add($"CHA:{old.Cha}→{stats.Cha}");
        if (old.Hp != stats.Hp) changes.Add($"HP:{old.Hp}→{stats.Hp}");
        if (old.MaxHp != stats.MaxHp) changes.Add($"MaxHP:{old.MaxHp}→{stats.MaxHp}");
        if (old.Mp != stats.Mp) changes.Add($"MP:{old.Mp}→{stats.Mp}");
        if (old.MaxMp != stats.MaxMp) changes.Add($"MaxMP:{old.MaxMp}→{stats.MaxMp}");
        if (old.Ac != stats.Ac) changes.Add($"AC:{old.Ac}→{stats.Ac}");
        if (old.Lfe != stats.Lfe) changes.Add($"LFE:{old.Lfe}→{stats.Lfe}");
        if (old.Dth != stats.Dth) changes.Add($"DTH:{old.Dth}→{stats.Dth}");
        var detail = changes.Any() ? string.Join(", ", changes) : null;

        _gameData.UpdateStats(id, stats);
        _adminLog.Add("캐릭터 능력치 수정", $"{character.Name} (ID:{id})", detail);
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    public IActionResult AddInventoryItem(int id, string itemName, Models.Domain.ItemGrade grade, int quantity, int enchant, bool isEquipped = false)
    {
        var character = _gameData.GetCharacter(id);
        if (character == null) return NotFound();
        _gameData.AddInventoryItem(id, itemName, grade, quantity, enchant, isEquipped);
        _adminLog.Add("인벤토리 아이템 추가", $"{character.Name} (ID:{id})", $"{itemName} x{quantity} +{enchant}");
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    public IActionResult AddAllItems(int id)
    {
        var character = _gameData.GetCharacter(id);
        if (character == null) return NotFound();
        var allItems = _gameData.GetAllItems();
        _gameData.AddAllItems(id);
        _adminLog.Add("인벤토리 전체 추가", $"{character.Name} (ID:{id})", $"{allItems.Count}종 추가");
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    public IActionResult UpdateInventoryItem(int id, int inventoryItemId, int quantity, int enchant, bool isEquipped = false)
    {
        var character = _gameData.GetCharacter(id);
        if (character == null) return NotFound();
        _gameData.UpdateInventoryItem(id, inventoryItemId, quantity, enchant, isEquipped);
        _adminLog.Add("인벤토리 아이템 수정", $"{character.Name} (ID:{id})", $"ItemID:{inventoryItemId} 수량:{quantity} 강화:+{enchant}");
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    public IActionResult RemoveInventoryItem(int id, int inventoryItemId)
    {
        var character = _gameData.GetCharacter(id);
        if (character == null) return NotFound();
        var item = character.Inventory.FirstOrDefault(i => i.Id == inventoryItemId);
        _gameData.RemoveInventoryItem(id, inventoryItemId);
        _adminLog.Add("인벤토리 아이템 삭제", $"{character.Name} (ID:{id})", item?.ItemName);
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    public IActionResult UpdateCurrency(int id, int adena, int diamond)
    {
        var character = _gameData.GetCharacter(id);
        if (character == null) return NotFound();

        var changes = new List<string>();
        if (character.Adena != adena) changes.Add($"아데나:{character.Adena:N0}→{adena:N0}");
        if (character.Diamond != diamond) changes.Add($"다이아:{character.Diamond:N0}→{diamond:N0}");
        var detail = changes.Any() ? string.Join(", ", changes) : null;

        _gameData.UpdateCurrency(id, adena, diamond);
        _adminLog.Add("캐릭터 재화 수정", $"{character.Name} (ID:{id})", detail);
        return RedirectToAction(nameof(Detail), new { id });
    }
}
