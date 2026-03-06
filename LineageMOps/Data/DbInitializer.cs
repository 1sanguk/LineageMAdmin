using LineageMOps.Models.Domain;

namespace LineageMOps.Data;

public static class DbInitializer
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        var db = serviceProvider.GetRequiredService<LineageMOpsDbContext>();
        db.Database.EnsureCreated();

        if (db.Accounts.Any()) return; // already seeded

        var mock = serviceProvider.GetRequiredService<MockDataStore>();

        // Seed Items
        db.Items.AddRange(mock.Items);
        db.SaveChanges();

        // Seed Accounts (without Sanctions first to avoid ID conflicts)
        var accounts = mock.Accounts.Select(a => new Account
        {
            Id = a.Id,
            UserId = a.UserId,
            UserName = a.UserName,
            Email = a.Email,
            Server = a.Server,
            RegisteredAt = a.RegisteredAt,
            LastLoginAt = a.LastLoginAt,
            IpAddress = a.IpAddress,
            Status = a.Status,
            Sanctions = new List<BannedRecord>()
        }).ToList();
        db.Accounts.AddRange(accounts);
        db.SaveChanges();

        // Seed BannedRecords
        var sanctions = mock.Accounts.SelectMany(a => a.Sanctions).ToList();
        db.BannedRecords.AddRange(sanctions);
        db.SaveChanges();

        // Seed Characters (without Inventory first)
        var characters = mock.Characters.Select(c => new Character
        {
            Id = c.Id,
            AccountId = c.AccountId,
            Name = c.Name,
            Server = c.Server,
            Class = c.Class,
            Level = c.Level,
            Experience = c.Experience,
            MaxExperience = c.MaxExperience,
            ClanName = c.ClanName,
            Adena = c.Adena,
            CreatedAt = c.CreatedAt,
            LastPlayedAt = c.LastPlayedAt,
            Stats = c.Stats,
            Inventory = new List<InventoryItem>()
        }).ToList();
        db.Characters.AddRange(characters);
        db.SaveChanges();

        // Seed InventoryItems
        var inventory = mock.Characters.SelectMany(c => c.Inventory).ToList();
        db.InventoryItems.AddRange(inventory);
        db.SaveChanges();

        // Seed Events
        db.Events.AddRange(mock.Events);
        db.SaveChanges();

        // Seed Notices
        db.Notices.AddRange(mock.Notices);
        db.SaveChanges();

        // Seed ServerLogs
        db.ServerLogs.AddRange(mock.Logs);
        db.SaveChanges();
    }
}
