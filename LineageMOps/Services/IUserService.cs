using LineageMOps.Models.Domain;
using LineageMOps.Models.ViewModels;

namespace LineageMOps.Services;

public interface IUserService
{
    List<Account> Search(string? query, string? server, AccountStatus? status, int page, int pageSize, out int totalCount);
    Account? GetById(int id);
    UserDetailViewModel? GetDetail(int id);
    void ApplySanction(SanctionFormViewModel form);
}
