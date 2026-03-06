namespace LineageMOps.Models.Domain;

public class BannedRecord
{
    public int Id { get; set; }
    public int AccountId { get; set; }
    public SanctionType Type { get; set; }
    public string Reason { get; set; } = "";
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string OperatorId { get; set; } = "";
    public bool IsActive { get; set; }
}
