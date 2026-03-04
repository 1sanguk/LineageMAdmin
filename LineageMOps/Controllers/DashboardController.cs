using LineageMOps.Models.ViewModels;
using LineageMOps.Services;
using Microsoft.AspNetCore.Mvc;

namespace LineageMOps.Controllers;

public class DashboardController : Controller
{
    private readonly IMonitoringService _monitoring;
    private readonly IUserService _users;
    private readonly IEventService _events;

    public DashboardController(IMonitoringService monitoring, IUserService users, IEventService events)
    {
        _monitoring = monitoring;
        _users = users;
        _events = events;
    }

    public IActionResult Index()
    {
        var servers = _monitoring.GetServerStatuses();
        var recentLogs = _monitoring.GetRecentLogs(10);
        var activeEvents = _events.GetEvents(Models.Domain.EventStatus.Active);

        var vm = new DashboardViewModel
        {
            Servers = servers,
            TotalOnlinePlayers = servers.Sum(s => s.CurrentPlayers),
            TodayNewAccounts = 23,
            TodayNewSanctions = 4,
            ActiveEvents = activeEvents.Count,
            RecentLogs = recentLogs
        };
        return View(vm);
    }
}
