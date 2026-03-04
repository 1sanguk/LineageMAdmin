namespace LineageMOps.Models.Domain;

public class Account
{
    public int Id { get; set; }
    public string UserId { get; set; } = "";
    public string UserName { get; set; } = "";
    public string Email { get; set; } = "";
    public string Server { get; set; } = "";
    public DateTime RegisteredAt { get; set; }
    public DateTime LastLoginAt { get; set; }
    public string IpAddress { get; set; } = "";
    public AccountStatus Status { get; set; }
    public List<SanctionRecord> Sanctions { get; set; } = new();
}

public enum AccountStatus
{
    Active,
    Suspended,
    Banned,
    Dormant
}

public class SanctionRecord
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

public enum SanctionType
{
    ChatBan,
    LoginRestriction,
    PermanentBan
}
