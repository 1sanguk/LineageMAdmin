using LineageMOps.Constants;
using LineageMOps.Data;
using LineageMOps.Models.Domain;
using LineageMOps.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LineageMOps.Services.Sql;

public class SqlUserService : IUserService
{
    private readonly LineageMOpsDbContext _db;

    public SqlUserService(LineageMOpsDbContext db) => _db = db;

    public PaginatedList<Account> Search(string? query, string? server, AccountStatus? status, int page, int pageSize)
    {
        var accounts = _db.Accounts.Include(a => a.Sanctions).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
            accounts = accounts.Where(a => a.UserId.Contains(query)
                                        || a.UserName.Contains(query)
                                        || a.Email.Contains(query));

        if (!string.IsNullOrWhiteSpace(server))
            accounts = accounts.Where(a => a.Server == server);

        if (status.HasValue)
            accounts = accounts.Where(a => a.Status == status.Value);

        var totalCount = accounts.Count();
        var items = accounts.OrderByDescending(a => a.LastLoginAt)
                            .Skip((page - 1) * pageSize)
                            .Take(pageSize)
                            .ToList();
        return PaginatedList<Account>.From(items, totalCount, page, pageSize);
    }

    public Account? GetById(int id) =>
        _db.Accounts.Include(a => a.Sanctions).FirstOrDefault(a => a.Id == id);

    public void ApplySanction(SanctionFormViewModel form)
    {
        var account = _db.Accounts.FirstOrDefault(a => a.Id == form.AccountId);
        if (account == null) return;

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
        _db.BannedRecords.Add(sanction);
        account.Status = form.Type == SanctionType.PermanentBan ? AccountStatus.Banned : AccountStatus.Suspended;
        _db.SaveChanges();
    }

    public int GetTodayNewAccountCount() =>
        _db.Accounts.Count(a => a.RegisteredAt.Date == DateTime.Today);
}
