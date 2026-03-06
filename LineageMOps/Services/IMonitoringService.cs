using LineageMOps.Models.Domain;
using LineageMOps.Models.ViewModels;

namespace LineageMOps.Services;

public interface IMonitoringService
{
    List<ServerStatus> GetServerStatuses();
    PaginatedList<ServerLog> GetLogs(LogType? type, string? server, string? search, int page, int pageSize);
    List<ServerLog> GetRecentLogs(int count = 20);
}
