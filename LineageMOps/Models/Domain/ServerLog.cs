namespace LineageMOps.Models.Domain;

public class ServerLog
{
    public int Id { get; set; }
    public string Server { get; set; } = "";
    public LogType Type { get; set; }
    public LogLevel Level { get; set; }
    public string Message { get; set; } = "";
    public string? UserId { get; set; }
    public string? CharacterName { get; set; }
    public DateTime Timestamp { get; set; }
    public string? IpAddress { get; set; }
    public string? Detail { get; set; }
}

public enum LogType
{
    Login,
    Logout,
    Chat,
    Trade,
    System,
    Audit,
    Error
}

public enum LogLevel
{
    Info,
    Warning,
    Error,
    Critical
}

public class ServerStatus
{
    public string Name { get; set; } = "";
    public string DisplayName { get; set; } = "";
    public ServerState State { get; set; }
    public int CurrentPlayers { get; set; }
    public int MaxPlayers { get; set; }
    public double CpuUsage { get; set; }
    public double MemoryUsage { get; set; }
    public double Uptime { get; set; }
}

public enum ServerState
{
    Online,
    Maintenance,
    Warning,
    Offline
}
