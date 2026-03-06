using LineageMOps.Models.Domain;
using LineageMOps.Models.ViewModels;
using LineageMOps.Services;
using Microsoft.AspNetCore.Mvc;

namespace LineageMOps.Controllers;

public class MonitoringController : Controller
{
    private readonly IMonitoringService _monitoring;
    private const int PageSize = 20;

    public MonitoringController(IMonitoringService monitoring)
    {
        _monitoring = monitoring;
    }

    public IActionResult Index(LogType? type, string? server, string? search, int page = 1)
    {
        var vm = new MonitoringViewModel
        {
            Logs = _monitoring.GetLogs(type, server, search, page, PageSize),
            Servers = _monitoring.GetServerStatuses(),
            FilterType = type,
            FilterServer = server,
            FilterSearch = search
        };
        return View(vm);
    }
}
