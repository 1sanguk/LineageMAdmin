namespace LineageMOps.Models.Domain;

public class AdminLog
{
    public int Id { get; set; }
    public string Action { get; set; } = "";
    public string Target { get; set; } = "";
    public string? Detail { get; set; }
    public string OperatorId { get; set; } = "op_001";
    public DateTime CreatedAt { get; set; } = DateTime.Now;
}
