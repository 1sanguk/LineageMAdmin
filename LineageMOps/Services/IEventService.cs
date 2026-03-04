using LineageMOps.Models.Domain;

namespace LineageMOps.Services;

public interface IEventService
{
    List<GameEvent> GetEvents(EventStatus? status = null);
    GameEvent? GetEvent(int id);
    void SaveEvent(GameEvent evt);
    void DeleteEvent(int id);

    List<Notice> GetNotices(bool includeUnpublished = false);
    Notice? GetNotice(int id);
    void SaveNotice(Notice notice);
    void DeleteNotice(int id);
}
