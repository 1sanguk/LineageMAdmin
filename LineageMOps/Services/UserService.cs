using LineageMOps.Data;
using LineageMOps.Models.Domain;
using LineageMOps.Models.ViewModels;

namespace LineageMOps.Services;

public class UserService : IUserService
{
    private readonly MockDataStore _store;

    public UserService(MockDataStore store) => _store = store;

    public List<Account> Search(string? query, string? server, AccountStatus? status, int page, int pageSize, out int totalCount)
    {
        var q = _store.Accounts.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(query))
            q = q.Where(a => a.UserId.Contains(query, StringComparison.OrdinalIgnoreCase)
                           || a.UserName.Contains(query, StringComparison.OrdinalIgnoreCase)
                           || a.Email.Contains(query, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(server))
            q = q.Where(a => a.Server == server);

        if (status.HasValue)
            q = q.Where(a => a.Status == status.Value);

        var list = q.OrderByDescending(a => a.LastLoginAt).ToList();
        totalCount = list.Count;
        return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }

    public Account? GetById(int id) => _store.Accounts.FirstOrDefault(a => a.Id == id);

    public UserDetailViewModel? GetDetail(int id)
    {
        var account = _store.Accounts.FirstOrDefault(a => a.Id == id);
        if (account == null) return null;

        return new UserDetailViewModel
        {
            Account = account,
            Characters = _store.Characters.Where(c => c.AccountId == id).ToList(),
            SanctionHistory = account.Sanctions.OrderByDescending(s => s.StartDate).ToList()
        };
    }

    public void ApplySanction(SanctionFormViewModel form)
    {
        var sanction = new BannedRecord
        {
            AccountId = form.AccountId,
            Type = form.Type,
            Reason = form.Reason,
            StartDate = DateTime.Now,
            EndDate = form.Type == SanctionType.PermanentBan ? null : DateTime.Now.AddDays(form.DurationDays ?? 7),
            OperatorId = "op_001",
            IsActive = true
        };
        _store.AddSanction(sanction);
    }
}
