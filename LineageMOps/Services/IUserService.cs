using LineageMOps.Models.Domain;
using LineageMOps.Models.ViewModels;

namespace LineageMOps.Services;

public interface IUserService
{
    PaginatedList<Account> Search(string? query, string? server, AccountStatus? status, int page, int pageSize);
    Account? GetById(int id);
    void ApplySanction(SanctionFormViewModel form);
    int GetTodayNewAccountCount();
}
