using LineageMOps.Data;
using LineageMOps.Models.Domain;

namespace LineageMOps.Services;

public class AdminLogService : IAdminLogService
{
    private readonly MockDataStore _store;

    public AdminLogService(MockDataStore store) => _store = store;

    public void Add(string action, string target, string? detail = null, string operatorId = "op_001")
    {
        var log = new AdminLog
        {
            Id = _store.AdminLogs.Any() ? _store.AdminLogs.Max(l => l.Id) + 1 : 1,
            Action = action,
            Target = target,
            Detail = detail,
            OperatorId = operatorId,
            CreatedAt = DateTime.Now
        };
        _store.AdminLogs.Insert(0, log);
    }

    public List<AdminLog> GetRecent(int count = 50) =>
        _store.AdminLogs.Take(count).ToList();
}
