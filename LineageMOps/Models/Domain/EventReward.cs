namespace LineageMOps.Models.Domain;

public class EventReward
{
    public int Id { get; set; }
    public RewardType Type { get; set; }
    public RewardTrigger Trigger { get; set; }
    public string? ItemName { get; set; }
    public long Amount { get; set; }
    public int? TargetMinLevel { get; set; }
    public int? TargetMaxLevel { get; set; }
    public string? ConditionDescription { get; set; }
}
