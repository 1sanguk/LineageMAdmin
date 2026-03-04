using LineageMOps.Data;
using LineageMOps.Models.Domain;

namespace LineageMOps.Services.Sql;

public class SqlEventService : IEventService
{
    private readonly LineageMOpsDbContext _db;

    public SqlEventService(LineageMOpsDbContext db) => _db = db;

    public List<GameEvent> GetEvents(EventStatus? status = null)
    {
        var q = _db.Events.AsQueryable();
        if (status.HasValue) q = q.Where(e => e.Status == status.Value);
        return q.OrderByDescending(e => e.StartDate).ToList();
    }

    public GameEvent? GetEvent(int id) => _db.Events.Find(id);

    public void SaveEvent(GameEvent evt)
    {
        if (evt.Id > 0)
        {
            evt.UpdatedAt = DateTime.Now;
            _db.Events.Update(evt);
        }
        else
        {
            evt.UpdatedAt = null;
            _db.Events.Add(evt);
        }
        _db.SaveChanges();
    }

    public void DeleteEvent(int id)
    {
        var evt = _db.Events.Find(id);
        if (evt != null)
        {
            _db.Events.Remove(evt);
            _db.SaveChanges();
        }
    }

    public List<Notice> GetNotices(bool includeUnpublished = false)
    {
        var q = _db.Notices.AsQueryable();
        if (!includeUnpublished) q = q.Where(n => n.IsPublished);
        return q.OrderByDescending(n => n.CreatedAt).ToList();
    }

    public Notice? GetNotice(int id) => _db.Notices.Find(id);

    public void SaveNotice(Notice notice)
    {
        if (notice.Id > 0)
        {
            notice.UpdatedAt = DateTime.Now;
            _db.Notices.Update(notice);
        }
        else
        {
            notice.UpdatedAt = null;
            _db.Notices.Add(notice);
        }
        _db.SaveChanges();
    }

    public void DeleteNotice(int id)
    {
        var notice = _db.Notices.Find(id);
        if (notice != null)
        {
            _db.Notices.Remove(notice);
            _db.SaveChanges();
        }
    }
}
