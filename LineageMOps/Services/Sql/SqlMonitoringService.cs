using LineageMOps.Data;
using LineageMOps.Models.Domain;
using LineageMOps.Models.ViewModels;

namespace LineageMOps.Services.Sql;

public class SqlMonitoringService : IMonitoringService
{
    private readonly LineageMOpsDbContext _db;
    private readonly MockDataStore _mock;

    public SqlMonitoringService(LineageMOpsDbContext db, MockDataStore mock)
    {
        _db = db;
        _mock = mock;
    }

    // Server statuses are runtime metrics not stored in DB — use mock data
    public List<ServerStatus> GetServerStatuses() => _mock.Servers;

    public PaginatedList<ServerLog> GetLogs(LogType? type, string? server, string? search, int page, int pageSize)
    {
        var logs = _db.ServerLogs.AsQueryable();

        if (type.HasValue)
            logs = logs.Where(l => l.Type == type.Value);

        if (!string.IsNullOrWhiteSpace(server))
            logs = logs.Where(l => l.Server == server);

        if (!string.IsNullOrWhiteSpace(search))
            logs = logs.Where(l => l.Message.Contains(search)
                                || (l.UserId != null && l.UserId.Contains(search)));

        var totalCount = logs.Count();
        var items = logs.OrderByDescending(l => l.Timestamp)
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();
        return PaginatedList<ServerLog>.From(items, totalCount, page, pageSize);
    }

    public List<ServerLog> GetRecentLogs(int count = 20) =>
        _db.ServerLogs.OrderByDescending(l => l.Timestamp).Take(count).ToList();
}
