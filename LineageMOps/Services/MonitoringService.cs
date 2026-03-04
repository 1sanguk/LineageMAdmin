using LineageMOps.Data;
using LineageMOps.Models.Domain;

namespace LineageMOps.Services;

public class MonitoringService : IMonitoringService
{
    private readonly MockDataStore _store;

    public MonitoringService(MockDataStore store) => _store = store;

    public List<ServerStatus> GetServerStatuses() => _store.Servers;

    public List<ServerLog> GetLogs(LogType? type, string? server, string? search, int page, int pageSize, out int totalCount)
    {
        var q = _store.Logs.AsEnumerable();

        if (type.HasValue)
            q = q.Where(l => l.Type == type.Value);

        if (!string.IsNullOrWhiteSpace(server))
            q = q.Where(l => l.Server == server);

        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(l => l.Message.Contains(search, StringComparison.OrdinalIgnoreCase)
                           || (l.UserId != null && l.UserId.Contains(search, StringComparison.OrdinalIgnoreCase)));

        var list = q.OrderByDescending(l => l.Timestamp).ToList();
        totalCount = list.Count;
        return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }

    public List<ServerLog> GetRecentLogs(int count = 20) =>
        _store.Logs.OrderByDescending(l => l.Timestamp).Take(count).ToList();
}
