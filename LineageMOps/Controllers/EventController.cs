using LineageMOps.Models.Domain;
using LineageMOps.Services;
using Microsoft.AspNetCore.Mvc;

namespace LineageMOps.Controllers;

public class EventController : Controller
{
    private readonly IEventService _eventService;
    private static readonly string[] AllServers = { "켄라우헬", "바츠", "기란", "오렌", "아덴", "글루디오", "디온" };

    public EventController(IEventService eventService) => _eventService = eventService;

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
        if (evt.Id == 0) evt.CreatedBy = "op_001";
        _eventService.SaveEvent(evt);
        TempData["Success"] = "이벤트가 저장되었습니다.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteEvent(int id)
    {
        _eventService.DeleteEvent(id);
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
        if (notice.Id == 0) notice.CreatedBy = "op_001";
        _eventService.SaveNotice(notice);
        TempData["Success"] = "공지가 저장되었습니다.";
        return RedirectToAction(nameof(Index), new { tab = "notices" });
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult DeleteNotice(int id)
    {
        _eventService.DeleteNotice(id);
        TempData["Success"] = "공지가 삭제되었습니다.";
        return RedirectToAction(nameof(Index), new { tab = "notices" });
    }
}
