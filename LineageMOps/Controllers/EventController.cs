using LineageMOps.Models.Domain;
using LineageMOps.Services;
using Microsoft.AspNetCore.Mvc;

namespace LineageMOps.Controllers;

public class EventController : Controller
{
    private readonly IEventService _eventService;
    private readonly IAdminLogService _adminLog;
    private static readonly string[] AllServers = { "켄라우헬", "바츠", "기란", "오렌", "아덴", "글루디오", "디온" };

    public EventController(IEventService eventService, IAdminLogService adminLog)
    {
        _eventService = eventService;
        _adminLog = adminLog;
    }

    public IActionResult Index(string tab = "events")
    {
        ViewBag.Tab = tab;
        ViewBag.Events = _eventService.GetEvents();
        ViewBag.Notices = _eventService.GetNotices(includeUnpublished: true);
        return View();
    }

    // ─── Events ──────────────────────────────────────────────
    [HttpGet]
    public IActionResult EditEvent(int? id)
    {
        var evt = id.HasValue ? _eventService.GetEvent(id.Value) : new GameEvent
        {
            StartDate = DateTime.Today,
            EndDate = DateTime.Today.AddDays(7),
            ApplicableServers = AllServers.ToList(),
            CreatedBy = "op_001",
            CreatedAt = DateTime.Now
        };
        if (evt == null) return NotFound();
        ViewBag.AllServers = AllServers;
        return View(evt);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditEvent(GameEvent evt, string[] selectedServers)
    {
        evt.ApplicableServers = selectedServers.ToList();
        if (string.IsNullOrWhiteSpace(evt.Title))
            ModelState.AddModelError(nameof(evt.Title), "제목을 입력하세요.");
        if (!ModelState.IsValid)
        {
            ViewBag.AllServers = AllServers;
            return View(evt);
        }
        bool isNew = evt.Id == 0;
        if (isNew) evt.CreatedBy = "op_001";
        _eventService.SaveEvent(evt);
        _adminLog.Add(isNew ? "이벤트 등록" : "이벤트 수정", evt.Title);
        TempData["Success"] = "이벤트가 저장되었습니다.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteEvent(int id)
    {
        var evt = _eventService.GetEvent(id);
        _eventService.DeleteEvent(id);
        _adminLog.Add("이벤트 삭제", evt?.Title ?? $"ID:{id}");
        TempData["Success"] = "이벤트가 삭제되었습니다.";
        return RedirectToAction(nameof(Index));
    }

    // ─── Notices ─────────────────────────────────────────────
    [HttpGet]
    public IActionResult EditNotice(int? id)
    {
        var notice = id.HasValue ? _eventService.GetNotice(id.Value) : new Notice
        {
            PublishDate = DateTime.Today,
            ApplicableServers = AllServers.ToList(),
            CreatedBy = "op_001",
            CreatedAt = DateTime.Now
        };
        if (notice == null) return NotFound();
        ViewBag.AllServers = AllServers;
        return View(notice);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult EditNotice(Notice notice, string[] selectedServers)
    {
        notice.ApplicableServers = selectedServers.ToList();
        if (string.IsNullOrWhiteSpace(notice.Title))
            ModelState.AddModelError(nameof(notice.Title), "제목을 입력하세요.");
        if (!ModelState.IsValid)
        {
            ViewBag.AllServers = AllServers;
            return View(notice);
        }
        bool isNew = notice.Id == 0;
        if (isNew) notice.CreatedBy = "op_001";
        _eventService.SaveNotice(notice);
        _adminLog.Add(isNew ? "공지 등록" : "공지 수정", notice.Title);
        TempData["Success"] = "공지가 저장되었습니다.";
        return RedirectToAction(nameof(Index), new { tab = "notices" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteNotice(int id)
    {
        var notice = _eventService.GetNotice(id);
        _eventService.DeleteNotice(id);
        _adminLog.Add("공지 삭제", notice?.Title ?? $"ID:{id}");
        TempData["Success"] = "공지가 삭제되었습니다.";
        return RedirectToAction(nameof(Index), new { tab = "notices" });
    }
}
