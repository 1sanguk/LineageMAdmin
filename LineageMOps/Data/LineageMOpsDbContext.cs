using System.Text.Json;
using LineageMOps.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace LineageMOps.Data;

public class LineageMOpsDbContext : DbContext
{
    public LineageMOpsDbContext(DbContextOptions<LineageMOpsDbContext> options) : base(options) { }

    public DbSet<Account> Accounts => Set<Account>();
    public DbSet<SanctionRecord> SanctionRecords => Set<SanctionRecord>();
    public DbSet<Character> Characters => Set<Character>();
    public DbSet<InventoryItem> InventoryItems => Set<InventoryItem>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<GameEvent> Events => Set<GameEvent>();
    public DbSet<Notice> Notices => Set<Notice>();
    public DbSet<ServerLog> ServerLogs => Set<ServerLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Account
        modelBuilder.Entity<Account>(entity =>
        {
            entity.HasKey(a => a.Id);
            entity.HasIndex(a => a.UserId).IsUnique();
            entity.HasIndex(a => new { a.Server, a.LastLoginAt });
            entity.Property(a => a.UserId).HasMaxLength(50).IsRequired();
            entity.Property(a => a.UserName).HasMaxLength(100).IsRequired();
            entity.Property(a => a.Email).HasMaxLength(200);
            entity.Property(a => a.Server).HasMaxLength(50);
            entity.Property(a => a.IpAddress).HasMaxLength(45);

            entity.HasMany(a => a.Sanctions)
                  .WithOne()
                  .HasForeignKey(s => s.AccountId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // SanctionRecord
        modelBuilder.Entity<SanctionRecord>(entity =>
        {
            entity.HasKey(s => s.Id);
            entity.Property(s => s.Reason).HasMaxLength(500);
            entity.Property(s => s.OperatorId).HasMaxLength(50);
        });

        // Character
        modelBuilder.Entity<Character>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasIndex(c => new { c.Name, c.Server });
            entity.Property(c => c.Name).HasMaxLength(100).IsRequired();
            entity.Property(c => c.Server).HasMaxLength(50);
            entity.Property(c => c.ClanName).HasMaxLength(100);

            entity.OwnsOne(c => c.Stats, s =>
            {
                s.Property(x => x.Str).HasColumnName("Stats_Str");
                s.Property(x => x.Dex).HasColumnName("Stats_Dex");
                s.Property(x => x.Con).HasColumnName("Stats_Con");
                s.Property(x => x.Wis).HasColumnName("Stats_Wis");
                s.Property(x => x.Int).HasColumnName("Stats_Int");
                s.Property(x => x.Cha).HasColumnName("Stats_Cha");
                s.Property(x => x.Hp).HasColumnName("Stats_Hp");
                s.Property(x => x.MaxHp).HasColumnName("Stats_MaxHp");
                s.Property(x => x.Mp).HasColumnName("Stats_Mp");
                s.Property(x => x.MaxMp).HasColumnName("Stats_MaxMp");
                s.Property(x => x.Ac).HasColumnName("Stats_Ac");
            });

            entity.HasMany(c => c.Inventory)
                  .WithOne()
                  .HasForeignKey(i => i.CharacterId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // InventoryItem
        modelBuilder.Entity<InventoryItem>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.ItemName).HasMaxLength(200);
        });

        // Item
        modelBuilder.Entity<Item>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.Property(i => i.Name).HasMaxLength(200).IsRequired();
            entity.Property(i => i.Description).HasMaxLength(1000);
        });

        // GameEvent
        modelBuilder.Entity<GameEvent>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasMaxLength(2000);
            entity.Property(e => e.CreatedBy).HasMaxLength(50);

            entity.Property(e => e.ApplicableServers)
                  .HasConversion(
                      v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                      v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new()
                  )
                  .HasColumnType("nvarchar(1000)");
        });

        // Notice
        modelBuilder.Entity<Notice>(entity =>
        {
            entity.HasKey(n => n.Id);
            entity.Property(n => n.Title).HasMaxLength(200).IsRequired();
            entity.Property(n => n.Content).HasMaxLength(4000);
            entity.Property(n => n.CreatedBy).HasMaxLength(50);

            entity.Property(n => n.ApplicableServers)
                  .HasConversion(
                      v => JsonSerializer.Serialize(v, (JsonSerializerOptions?)null),
                      v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions?)null) ?? new()
                  )
                  .HasColumnType("nvarchar(1000)");
        });

        // ServerLog
        modelBuilder.Entity<ServerLog>(entity =>
        {
            entity.HasKey(l => l.Id);
            entity.HasIndex(l => new { l.Timestamp, l.Type, l.Server });
            entity.Property(l => l.Server).HasMaxLength(50);
            entity.Property(l => l.Message).HasMaxLength(2000);
            entity.Property(l => l.UserId).HasMaxLength(50);
            entity.Property(l => l.CharacterName).HasMaxLength(100);
            entity.Property(l => l.IpAddress).HasMaxLength(45);
            entity.Property(l => l.Detail).HasMaxLength(1000);
        });
    }
}
