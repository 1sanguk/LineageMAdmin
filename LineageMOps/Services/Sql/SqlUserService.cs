using LineageMOps.Data;
using LineageMOps.Models.Domain;
using LineageMOps.Models.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace LineageMOps.Services.Sql;

public class SqlUserService : IUserService
{
    private readonly LineageMOpsDbContext _db;

    public SqlUserService(LineageMOpsDbContext db) => _db = db;

    public List<Account> Search(string? query, string? server, AccountStatus? status, int page, int pageSize, out int totalCount)
    {
        var q = _db.Accounts.Include(a => a.Sanctions).AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
            q = q.Where(a => a.UserId.Contains(query)
                           || a.UserName.Contains(query)
                           || a.Email.Contains(query));

        if (!string.IsNullOrWhiteSpace(server))
            q = q.Where(a => a.Server == server);

        if (status.HasValue)
            q = q.Where(a => a.Status == status.Value);

        totalCount = q.Count();
        return q.OrderByDescending(a => a.LastLoginAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
    }

    public Account? GetById(int id) =>
        _db.Accounts.Include(a => a.Sanctions).FirstOrDefault(a => a.Id == id);

    public UserDetailViewModel? GetDetail(int id)
    {
        var account = _db.Accounts.Include(a => a.Sanctions).FirstOrDefault(a => a.Id == id);
        if (account == null) return null;

        return new UserDetailViewModel
        {
            Account = account,
            Characters = _db.Characters.Where(c => c.AccountId == id).ToList(),
            SanctionHistory = account.Sanctions.OrderByDescending(s => s.StartDate).ToList()
        };
    }

    public void ApplySanction(SanctionFormViewModel form)
    {
        var account = _db.Accounts.FirstOrDefault(a => a.Id == form.AccountId);
        if (account == null) return;

        var sanction = new SanctionRecord
        {
            AccountId = form.AccountId,
            Type = form.Type,
            Reason = form.Reason,
            StartDate = DateTime.Now,
            EndDate = form.Type == SanctionType.PermanentBan ? null : DateTime.Now.AddDays(form.DurationDays ?? 7),
            OperatorId = "op_001",
            IsActive = true
        };

        _db.SanctionRecords.Add(sanction);
        account.Status = form.Type == SanctionType.PermanentBan ? AccountStatus.Banned : AccountStatus.Suspended;
        _db.SaveChanges();
    }
}
