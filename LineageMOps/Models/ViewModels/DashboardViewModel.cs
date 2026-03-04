using LineageMOps.Models.Domain;

namespace LineageMOps.Models.ViewModels;

public class DashboardViewModel
{
    public List<ServerStatus> Servers { get; set; } = new();
    public int TotalOnlinePlayers { get; set; }
    public int TodayNewAccounts { get; set; }
    public int TodayNewSanctions { get; set; }
    public int ActiveEvents { get; set; }
    public List<ServerLog> RecentLogs { get; set; } = new();
}
