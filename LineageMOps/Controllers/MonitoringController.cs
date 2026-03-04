using LineageMOps.Models.Domain;
using LineageMOps.Models.ViewModels;
using LineageMOps.Services;
using Microsoft.AspNetCore.Mvc;

namespace LineageMOps.Controllers;

public class MonitoringController : Controller
{
    private readonly IMonitoringService _monitoring;
    private readonly IAdminLogService _adminLog;
    private const int PageSize = 20;

    public MonitoringController(IMonitoringService monitoring, IAdminLogService adminLog)
    {
        _monitoring = monitoring;
        _adminLog = adminLog;
    }

    public IActionResult Index(LogType? type, string? server, string? search, int page = 1)
    {
        var logs = _monitoring.GetLogs(type, server, search, page, PageSize, out int totalCount);
        var servers = _monitoring.GetServerStatuses();
        ViewBag.Servers = servers;
        ViewBag.Type = type;
        ViewBag.Server = server;
        ViewBag.Search = search;
        ViewBag.AdminLogs = _adminLog.GetRecent(50);
        var paged = new PaginatedList<ServerLog>
        {
            Items = logs,
            PageIndex = page,
            TotalPages = (int)Math.Ceiling(totalCount / (double)PageSize),
            TotalCount = totalCount,
            PageSize = PageSize
        };
        return View(paged);
    }
}
