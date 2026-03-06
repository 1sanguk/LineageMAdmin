using LineageMOps.Models.Domain;
using LineageMOps.Models.ViewModels;
using LineageMOps.Services;
using Microsoft.AspNetCore.Mvc;

namespace LineageMOps.Controllers;

public class GameDataController : Controller
{
    private readonly IGameDataService _gameDataService;
    private readonly IUserService _userService;
    private readonly IAdminLogService _adminLog;
    private const int PageSize = 15;

    public GameDataController(IGameDataService gameDataService, IUserService userService, IAdminLogService adminLog)
    {
        _gameDataService = gameDataService;
        _userService = userService;
        _adminLog = adminLog;
    }

    public IActionResult Index(string? q, string? server, string? cls, int page = 1)
    {
        var paged = _gameDataService.SearchCharacters(q, server, cls, page, PageSize);
        ViewBag.Query = q;
        ViewBag.Server = server;
        ViewBag.Class = cls;
        return View(paged);
    }

    public IActionResult Detail(int id)
    {
        var character = _gameDataService.GetCharacter(id);
        if (character == null) return NotFound();

        var vm = new CharacterDetailViewModel
        {
            Character = character,
            Account = _userService.GetById(character.AccountId),
            AllItems = _gameDataService.GetAllItems()
        };
        return View(vm);
    }

    [HttpPost]
    public IActionResult UpdateLevelExp(int id, int level, long experience)
    {
        var character = _gameDataService.GetCharacter(id);
        if (character == null) return NotFound();

        var changes = new List<string>();
        if (character.Level != level) changes.Add($"레벨:{character.Level}→{level}");
        if (character.Experience != experience) changes.Add($"경험치:{character.Experience:N0}→{experience:N0}");
        var detail = changes.Any() ? string.Join(", ", changes) : null;

        _gameDataService.UpdateLevelExp(id, level, experience);
        _adminLog.Add("레벨/경험치 수정", $"{character.Name} (ID:{id})", detail);
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    public IActionResult UpdateStats(int id, CharacterStats stats)
    {
        var character = _gameDataService.GetCharacter(id);
        if (character == null) return NotFound();

        var changes = _gameDataService.BuildStatChanges(character.Stats, stats);
        var detail = changes.Any() ? string.Join(", ", changes) : null;

        _gameDataService.UpdateStats(id, stats);
        _adminLog.Add("캐릭터 능력치 수정", $"{character.Name} (ID:{id})", detail);
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    public IActionResult AddInventoryItem(int id, string itemName, ItemGrade grade, int quantity, int enchant, bool isEquipped = false)
    {
        var character = _gameDataService.GetCharacter(id);
        if (character == null) return NotFound();
        _gameDataService.AddInventoryItem(id, itemName, grade, quantity, enchant, isEquipped);
        _adminLog.Add("인벤토리 아이템 추가", $"{character.Name} (ID:{id})", $"{itemName} x{quantity} +{enchant}");
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    public IActionResult AddAllItems(int id)
    {
        var character = _gameDataService.GetCharacter(id);
        if (character == null) return NotFound();
        var allItems = _gameDataService.GetAllItems();
        _gameDataService.AddAllItems(id);
        _adminLog.Add("인벤토리 전체 추가", $"{character.Name} (ID:{id})", $"{allItems.Count}종 추가");
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    public IActionResult UpdateInventoryItem(int id, int inventoryItemId, int quantity, int enchant, bool isEquipped = false)
    {
        var character = _gameDataService.GetCharacter(id);
        if (character == null) return NotFound();
        _gameDataService.UpdateInventoryItem(id, inventoryItemId, quantity, enchant, isEquipped);
        _adminLog.Add("인벤토리 아이템 수정", $"{character.Name} (ID:{id})", $"ItemID:{inventoryItemId} 수량:{quantity} 강화:+{enchant}");
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    public IActionResult RemoveInventoryItem(int id, int inventoryItemId)
    {
        var character = _gameDataService.GetCharacter(id);
        if (character == null) return NotFound();
        var item = character.Inventory.FirstOrDefault(i => i.Id == inventoryItemId);
        _gameDataService.RemoveInventoryItem(id, inventoryItemId);
        _adminLog.Add("인벤토리 아이템 삭제", $"{character.Name} (ID:{id})", item?.ItemName);
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    public IActionResult UpdateCurrency(int id, int adena, int diamond)
    {
        var character = _gameDataService.GetCharacter(id);
        if (character == null) return NotFound();

        var changes = new List<string>();
        if (character.Adena != adena) changes.Add($"아데나:{character.Adena:N0}→{adena:N0}");
        if (character.Diamond != diamond) changes.Add($"다이아:{character.Diamond:N0}→{diamond:N0}");
        var detail = changes.Any() ? string.Join(", ", changes) : null;

        _gameDataService.UpdateCurrency(id, adena, diamond);
        _adminLog.Add("캐릭터 재화 수정", $"{character.Name} (ID:{id})", detail);
        return RedirectToAction(nameof(Detail), new { id });
    }
}
