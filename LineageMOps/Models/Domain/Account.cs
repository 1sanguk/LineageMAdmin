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
    public List<BannedRecord> Sanctions { get; set; } = new();
}
