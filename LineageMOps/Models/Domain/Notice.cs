namespace LineageMOps.Models.Domain;

public class Notice
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Content { get; set; } = "";
    public NoticeType Type { get; set; }
    public bool IsPublished { get; set; }
    public DateTime PublishDate { get; set; }
    public DateTime? ExpireDate { get; set; }
    public List<string> ApplicableServers { get; set; } = new();
    public string CreatedBy { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public enum NoticeType
{
    General,
    Maintenance,
    Update,
    Event,
    Urgent
}
