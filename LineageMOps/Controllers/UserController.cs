using LineageMOps.Models.Domain;
using LineageMOps.Models.ViewModels;
using LineageMOps.Services;
using Microsoft.AspNetCore.Mvc;

namespace LineageMOps.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;
    private readonly IGameDataService _gameDataService;
    private readonly IAdminLogService _adminLog;
    private const int PageSize = 15;

    public UserController(IUserService userService, IGameDataService gameDataService, IAdminLogService adminLog)
    {
        _userService = userService;
        _gameDataService = gameDataService;
        _adminLog = adminLog;
    }

    public IActionResult Index(string? q, string? server, AccountStatus? status, int page = 1)
    {
        var paged = _userService.Search(q, server, status, page, PageSize);
        ViewBag.Query = q;
        ViewBag.Server = server;
        ViewBag.Status = status;
        return View(paged);
    }

    public IActionResult Detail(int id)
    {
        var account = _userService.GetById(id);
        if (account == null) return NotFound();

        var vm = new UserDetailViewModel
        {
            Account = account,
            Characters = _gameDataService.GetCharactersByAccountId(id),
            SanctionHistory = account.Sanctions.OrderByDescending(s => s.StartDate).ToList()
        };
        return View(vm);
    }

    [HttpGet]
    public IActionResult Sanction(int id)
    {
        var account = _userService.GetById(id);
        if (account == null) return NotFound();
        var vm = new SanctionFormViewModel
        {
            AccountId = account.Id,
            UserId = account.UserId
        };
        return View(vm);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Sanction(SanctionFormViewModel form)
    {
        if (!ModelState.IsValid)
            return View(form);

        _userService.ApplySanction(form);
        var sanctionText = form.Type switch
        {
            SanctionType.ChatBan => "채팅 금지",
            SanctionType.LoginRestriction => "접속 제한",
            SanctionType.PermanentBan => "영구 정지",
            _ => form.Type.ToString()
        };
        _adminLog.Add("계정 제재 적용", $"{form.UserId} (ID:{form.AccountId})", $"{sanctionText} / 사유: {form.Reason}");
        TempData["Success"] = $"유저 [{form.UserId}]에게 제재가 적용되었습니다.";
        return RedirectToAction(nameof(Detail), new { id = form.AccountId });
    }
}
