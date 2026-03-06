using LineageMOps.Data;
using LineageMOps.Models.Domain;
using LineageMOps.Models.ViewModels;

namespace LineageMOps.Services;

public class MonitoringService : IMonitoringService
{
    private readonly MockDataStore _store;

    public MonitoringService(MockDataStore store) => _store = store;

    public List<ServerStatus> GetServerStatuses() => _store.Servers;

    public PaginatedList<ServerLog> GetLogs(LogType? type, string? server, string? search, int page, int pageSize)
    {
        var logs = _store.Logs.AsEnumerable();

        if (type.HasValue)
            logs = logs.Where(l => l.Type == type.Value);

        if (!string.IsNullOrWhiteSpace(server))
            logs = logs.Where(l => l.Server == server);

        if (!string.IsNullOrWhiteSpace(search))
            logs = logs.Where(l => l.Message.Contains(search, StringComparison.OrdinalIgnoreCase)
                                || (l.UserId != null && l.UserId.Contains(search, StringComparison.OrdinalIgnoreCase)));

        var sorted = logs.OrderByDescending(l => l.Timestamp).ToList();
        var items = sorted.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return PaginatedList<ServerLog>.From(items, sorted.Count, page, pageSize);
    }

    public List<ServerLog> GetRecentLogs(int count = 20) =>
        _store.Logs.OrderByDescending(l => l.Timestamp).Take(count).ToList();
}
