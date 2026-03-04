using LineageMOps.Models.Domain;
using LineageMOps.Models.ViewModels;
using LineageMOps.Services;
using Microsoft.AspNetCore.Mvc;

namespace LineageMOps.Controllers;

public class MonitoringController : Controller
{
    private readonly IMonitoringService _monitoring;
    private const int PageSize = 20;

    public MonitoringController(IMonitoringService monitoring) => _monitoring = monitoring;

    public IActionResult Index(LogType? type, string? server, string? search, int page = 1)
    {
        var logs = _monitoring.GetLogs(type, server, search, page, PageSize, out int totalCount);
        var servers = _monitoring.GetServerStatuses();
        ViewBag.Servers = servers;
        ViewBag.Type = type;
        ViewBag.Server = server;
        ViewBag.Search = search;
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
