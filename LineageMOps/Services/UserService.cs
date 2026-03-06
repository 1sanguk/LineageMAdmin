using LineageMOps.Constants;
using LineageMOps.Data;
using LineageMOps.Models.Domain;
using LineageMOps.Models.ViewModels;

namespace LineageMOps.Services;

public class UserService : IUserService
{
    private readonly MockDataStore _store;

    public UserService(MockDataStore store) => _store = store;

    public PaginatedList<Account> Search(string? query, string? server, AccountStatus? status, int page, int pageSize)
    {
        var accounts = _store.Accounts.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(query))
            accounts = accounts.Where(a => a.UserId.Contains(query, StringComparison.OrdinalIgnoreCase)
                                        || a.UserName.Contains(query, StringComparison.OrdinalIgnoreCase)
                                        || a.Email.Contains(query, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(server))
            accounts = accounts.Where(a => a.Server == server);

        if (status.HasValue)
            accounts = accounts.Where(a => a.Status == status.Value);

        var sorted = accounts.OrderByDescending(a => a.LastLoginAt).ToList();
        var items = sorted.Skip((page - 1) * pageSize).Take(pageSize).ToList();
        return PaginatedList<Account>.From(items, sorted.Count, page, pageSize);
    }

    public Account? GetById(int id) => _store.Accounts.FirstOrDefault(a => a.Id == id);

    public void ApplySanction(SanctionFormViewModel form)
    {
        var sanction = new BannedRecord
        {
            AccountId = form.AccountId,
            Type = form.Type,
            Reason = form.Reason,
            StartDate = DateTime.Now,
            EndDate = form.Type == SanctionType.PermanentBan ? null : DateTime.Now.AddDays(form.DurationDays ?? 7),
            OperatorId = AppConstants.MockOperatorId,
            IsActive = true
        };
        _store.AddSanction(sanction);
    }

    public int GetTodayNewAccountCount() =>
        _store.Accounts.Count(a => a.RegisteredAt.Date == DateTime.Today);
}
