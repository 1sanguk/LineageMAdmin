using LineageMOps.Models.Domain;

namespace LineageMOps.Services;

public interface IAdminLogService
{
    void Add(string action, string target, string? detail = null, string operatorId = "op_001");
    List<AdminLog> GetRecent(int count = 50);
}
