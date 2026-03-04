namespace LineageMOps.Models.Domain;

public class GameEvent
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public EventType Type { get; set; }
    public EventStatus Status { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<string> ApplicableServers { get; set; } = new();
    public string CreatedBy { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public enum EventType
{
    Exp,
    Drop,
    Login,
    Special,
    Holiday
}

public enum EventStatus
{
    Scheduled,
    Active,
    Ended,
    Cancelled
}
