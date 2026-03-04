using LineageMOps.Data;
using LineageMOps.Models.Domain;

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

    public List<ServerLog> GetLogs(LogType? type, string? server, string? search, int page, int pageSize, out int totalCount)
    {
        var q = _db.ServerLogs.AsQueryable();

        if (type.HasValue)
            q = q.Where(l => l.Type == type.Value);

        if (!string.IsNullOrWhiteSpace(server))
            q = q.Where(l => l.Server == server);

        if (!string.IsNullOrWhiteSpace(search))
            q = q.Where(l => l.Message.Contains(search)
                           || (l.UserId != null && l.UserId.Contains(search)));

        totalCount = q.Count();
        return q.OrderByDescending(l => l.Timestamp)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
    }

    public List<ServerLog> GetRecentLogs(int count = 20) =>
        _db.ServerLogs.OrderByDescending(l => l.Timestamp).Take(count).ToList();
}
