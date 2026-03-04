using LineageMOps.Models.Domain;

namespace LineageMOps.Services;

public interface IMonitoringService
{
    List<ServerStatus> GetServerStatuses();
    List<ServerLog> GetLogs(LogType? type, string? server, string? search, int page, int pageSize, out int totalCount);
    List<ServerLog> GetRecentLogs(int count = 20);
}
