using LineageMOps.Constants;
using LineageMOps.Data;
using LineageMOps.Models.Domain;

namespace LineageMOps.Services.Sql;

public class SqlAdminLogService : IAdminLogService
{
    private readonly LineageMOpsDbContext _db;

    public SqlAdminLogService(LineageMOpsDbContext db) => _db = db;

    public void Add(string action, string target, string? detail = null, string operatorId = AppConstants.MockOperatorId)
    {
        _db.AdminLogs.Add(new AdminLog
        {
            Action = action,
            Target = target,
            Detail = detail,
            OperatorId = operatorId,
            CreatedAt = DateTime.Now
        });
        _db.SaveChanges();
    }

    public List<AdminLog> GetRecent(int count = 50) =>
        _db.AdminLogs.OrderByDescending(l => l.CreatedAt).Take(count).ToList();

    public int GetTodayNewSanctionCount() =>
        _db.AdminLogs.Count(l => l.Action.Contains("제재") && l.CreatedAt.Date == DateTime.Today);
}
