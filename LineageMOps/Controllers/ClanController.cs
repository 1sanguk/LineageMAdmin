using LineageMOps.Models.Domain;
using LineageMOps.Services;
using Microsoft.AspNetCore.Mvc;

namespace LineageMOps.Controllers;

public class ClanController : Controller
{
    private readonly IClanService _clanService;
    private readonly IAdminLogService _adminLog;
    private const int PageSize = 15;

    public ClanController(IClanService clanService, IAdminLogService adminLog)
    {
        _clanService = clanService;
        _adminLog    = adminLog;
    }

    public IActionResult Index(string? q, string? server, int? level, int page = 1)
    {
        var paged = _clanService.Search(q, server, level, page, PageSize);
        ViewBag.Query  = q;
        ViewBag.Server = server;
        ViewBag.Level  = level;
        return View(paged);
    }

    public IActionResult Detail(int id)
    {
        var vm = _clanService.GetDetail(id);
        if (vm == null) return NotFound();
        return View(vm);
    }

    [HttpPost]
    public IActionResult UpdateNotice(int id, string notice)
    {
        if (!_clanService.UpdateNotice(id, notice, out var clan))
            return NotFound();

        _adminLog.Add("혈맹 공지 수정", $"{clan!.Name} (ID:{id})", null);
        TempData["Success"] = "혈맹 공지가 수정되었습니다.";
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    public IActionResult UpdateIntroduction(int id, string introduction)
    {
        if (!_clanService.UpdateIntroduction(id, introduction, out var clan))
            return NotFound();

        _adminLog.Add("혈맹 소개글 수정", $"{clan!.Name} (ID:{id})", null);
        TempData["Success"] = "혈맹 소개글이 수정되었습니다.";
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    public IActionResult UpdateBasicInfo(int id, int level, long reputation, long experience,
                                         bool hasAjit, bool hasCastle, bool isAtWar,
                                         int warWins, int warLosses)
    {
        if (!_clanService.UpdateBasicInfo(id, level, reputation, experience,
                hasAjit, hasCastle, isAtWar, warWins, warLosses, out var clan))
            return NotFound();

        _adminLog.Add("혈맹 기본 정보 수정", $"{clan!.Name} (ID:{id})",
            $"레벨:{level} 명성:{reputation:N0} 경험치:{experience:N0}");
        TempData["Success"] = "혈맹 기본 정보가 수정되었습니다.";
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    public IActionResult UpdateSettings(int id, JoinPolicy joinPolicy, int bloodOathLevel)
    {
        if (!_clanService.UpdateSettings(id, joinPolicy, bloodOathLevel, out var clan))
            return NotFound();

        _adminLog.Add("혈맹 설정 수정", $"{clan!.Name} (ID:{id})",
            $"가입정책:{joinPolicy} 피의서약:{bloodOathLevel}단계");
        TempData["Success"] = "혈맹 설정이 수정되었습니다.";
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    public IActionResult AddRival(int id, string rivalName)
    {
        var clan = _clanService.GetById(id);
        if (clan == null) return NotFound();
        _clanService.AddRival(id, rivalName);
        _adminLog.Add("적대 혈맹 추가", $"{clan.Name} (ID:{id})", rivalName);
        TempData["Success"] = $"'{rivalName}'을(를) 적대 혈맹으로 등록했습니다.";
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    public IActionResult RemoveRival(int id, string rivalName)
    {
        var clan = _clanService.GetById(id);
        if (clan == null) return NotFound();
        _clanService.RemoveRival(id, rivalName);
        _adminLog.Add("적대 혈맹 제거", $"{clan.Name} (ID:{id})", rivalName);
        TempData["Success"] = $"'{rivalName}' 적대 관계를 해제했습니다.";
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    public IActionResult AddAlly(int id, string allyName)
    {
        var clan = _clanService.GetById(id);
        if (clan == null) return NotFound();
        _clanService.AddAlly(id, allyName);
        _adminLog.Add("동맹 혈맹 추가", $"{clan.Name} (ID:{id})", allyName);
        TempData["Success"] = $"'{allyName}'을(를) 동맹 혈맹으로 등록했습니다.";
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    public IActionResult RemoveAlly(int id, string allyName)
    {
        var clan = _clanService.GetById(id);
        if (clan == null) return NotFound();
        _clanService.RemoveAlly(id, allyName);
        _adminLog.Add("동맹 혈맹 제거", $"{clan.Name} (ID:{id})", allyName);
        TempData["Success"] = $"'{allyName}' 동맹 관계를 해제했습니다.";
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    public IActionResult KickMember(int id, int characterId)
    {
        var clan = _clanService.GetById(id);
        if (clan == null) return NotFound();
        if (!_clanService.KickMember(id, characterId, out var memberName))
        {
            TempData["Error"] = "혈맹원 추방에 실패했습니다. (군주는 추방 불가)";
            return RedirectToAction(nameof(Detail), new { id });
        }
        _adminLog.Add("혈맹원 추방", $"{memberName} (혈맹:{clan.Name})", null);
        TempData["Success"] = $"'{memberName}' 혈맹원을 추방했습니다.";
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    public IActionResult ChangeMemberRank(int id, int characterId, ClanRank newRank)
    {
        var clan = _clanService.GetById(id);
        if (clan == null) return NotFound();
        if (!_clanService.ChangeMemberRank(id, characterId, newRank, out var memberName))
            return NotFound();

        _adminLog.Add("혈맹원 계급 변경", $"{memberName} (혈맹:{clan.Name})", $"→{newRank}");
        TempData["Success"] = $"'{memberName}' 계급을 변경했습니다.";
        return RedirectToAction(nameof(Detail), new { id });
    }

    [HttpPost]
    public IActionResult Disband(int id)
    {
        if (!_clanService.Disband(id, out var clan))
            return NotFound();

        _adminLog.Add("혈맹 강제 해산", $"{clan!.Name} (ID:{id})", null);
        TempData["Success"] = $"혈맹 '{clan.Name}'이(가) 강제 해산되었습니다.";
        return RedirectToAction(nameof(Index));
    }
}
