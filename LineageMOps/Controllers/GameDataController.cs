using LineageMOps.Models.Domain;
using LineageMOps.Models.ViewModels;
using LineageMOps.Services;
using Microsoft.AspNetCore.Mvc;

namespace LineageMOps.Controllers;

public class GameDataController : Controller
{
    private readonly IGameDataService _gameData;
    private readonly IUserService _users;
    private const int PageSize = 15;

    public GameDataController(IGameDataService gameData, IUserService users)
    {
        _gameData = gameData;
        _users = users;
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
        return View(character);
    }
}
