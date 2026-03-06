using LineageMOps.Constants;
using LineageMOps.Models.Domain;

namespace LineageMOps.Services;

public interface IAdminLogService
{
    void Add(string action, string target, string? detail = null, string operatorId = AppConstants.MockOperatorId);
    List<AdminLog> GetRecent(int count = 50);
    int GetTodayNewSanctionCount();
}
