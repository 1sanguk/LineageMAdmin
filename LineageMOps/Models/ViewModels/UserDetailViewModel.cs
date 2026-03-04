using LineageMOps.Models.Domain;

namespace LineageMOps.Models.ViewModels;

public class UserDetailViewModel
{
    public Account Account { get; set; } = null!;
    public List<Character> Characters { get; set; } = new();
    public List<SanctionRecord> SanctionHistory { get; set; } = new();
}

public class SanctionFormViewModel
{
    public int AccountId { get; set; }
    public string UserId { get; set; } = "";
    public SanctionType Type { get; set; }
    public string Reason { get; set; } = "";
    public int? DurationDays { get; set; }
}
