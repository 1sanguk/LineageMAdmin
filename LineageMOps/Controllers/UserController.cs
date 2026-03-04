using LineageMOps.Models.Domain;
using LineageMOps.Models.ViewModels;
using LineageMOps.Services;
using Microsoft.AspNetCore.Mvc;

namespace LineageMOps.Controllers;

public class UserController : Controller
{
    private readonly IUserService _userService;
    private const int PageSize = 15;

    public UserController(IUserService userService) => _userService = userService;

    public IActionResult Index(string? q, string? server, AccountStatus? status, int page = 1)
    {
        var accounts = _userService.Search(q, server, status, page, PageSize, out int totalCount);
        ViewBag.Query = q;
        ViewBag.Server = server;
        ViewBag.Status = status;
        ViewBag.Paged = PaginatedList<Account>.Create(
            accounts.Concat(Enumerable.Empty<Account>()), 1, PageSize);
        // Already paginated by service, just wrap
        var paged = new PaginatedList<Account>
        {
            Items = accounts,
            PageIndex = page,
            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize),
            TotalCount = totalCount,
            PageSize = PageSize
        };
        return View(paged);
    }

    public IActionResult Detail(int id)
    {
        var vm = _userService.GetDetail(id);
        if (vm == null) return NotFound();
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
        TempData["Success"] = $"유저 [{form.UserId}]에게 제재가 적용되었습니다.";
        return RedirectToAction(nameof(Detail), new { id = form.AccountId });
    }
}
