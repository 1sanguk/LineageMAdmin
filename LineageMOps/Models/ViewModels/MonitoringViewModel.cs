using LineageMOps.Constants;
using LineageMOps.Models.Domain;

namespace LineageMOps.Models.ViewModels;

public class MonitoringViewModel
{
    public PaginatedList<ServerLog> Logs { get; set; } = new();
    public List<ServerStatus> Servers { get; set; } = new();
    public LogType? FilterType { get; set; }
    public string? FilterServer { get; set; }
    public string? FilterSearch { get; set; }
    public string[] AllServerNames { get; set; } = GameConstants.ServerNames;
}
