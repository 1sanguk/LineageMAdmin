namespace LineageMOps.Models.Domain;

public class GameConstraint
{
    public int Id { get; set; }
    public ConstraintCategory Category { get; set; }
    public string Key { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public string Description { get; set; } = "";
    public ConstraintValueType ValueType { get; set; }
    public string Value { get; set; } = "";
    public string DefaultValue { get; set; } = "";
    public double? Min { get; set; }
    public double? Max { get; set; }
    public string? Unit { get; set; }
    public DateTime UpdatedAt { get; set; } = DateTime.Now;
}
