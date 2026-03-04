using LineageMOps.Data;
using LineageMOps.Models.Domain;

namespace LineageMOps.Services;

public class EventService : IEventService
{
    private readonly MockDataStore _store;

    public EventService(MockDataStore store) => _store = store;

    public List<GameEvent> GetEvents(EventStatus? status = null)
    {
        var q = _store.Events.AsEnumerable();
        if (status.HasValue) q = q.Where(e => e.Status == status.Value);
        return q.OrderByDescending(e => e.StartDate).ToList();
    }

    public GameEvent? GetEvent(int id) => _store.Events.FirstOrDefault(e => e.Id == id);

    public void SaveEvent(GameEvent evt)
    {
        evt.UpdatedAt = evt.Id > 0 ? DateTime.Now : null;
        _store.SaveEvent(evt);
    }

    public void DeleteEvent(int id) => _store.DeleteEvent(id);

    public List<Notice> GetNotices(bool includeUnpublished = false)
    {
        var q = _store.Notices.AsEnumerable();
        if (!includeUnpublished) q = q.Where(n => n.IsPublished);
        return q.OrderByDescending(n => n.CreatedAt).ToList();
    }

    public Notice? GetNotice(int id) => _store.Notices.FirstOrDefault(n => n.Id == id);

    public void SaveNotice(Notice notice)
    {
        notice.UpdatedAt = notice.Id > 0 ? DateTime.Now : null;
        _store.SaveNotice(notice);
    }

    public void DeleteNotice(int id) => _store.DeleteNotice(id);
}
