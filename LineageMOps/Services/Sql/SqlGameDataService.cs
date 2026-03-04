using LineageMOps.Data;
using LineageMOps.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace LineageMOps.Services.Sql;

public class SqlGameDataService : IGameDataService
{
    private readonly LineageMOpsDbContext _db;

    public SqlGameDataService(LineageMOpsDbContext db) => _db = db;

    public List<Character> SearchCharacters(string? query, string? server, string? cls, int page, int pageSize, out int totalCount)
    {
        var q = _db.Characters.AsQueryable();

        if (!string.IsNullOrWhiteSpace(query))
            q = q.Where(c => c.Name.Contains(query));

        if (!string.IsNullOrWhiteSpace(server))
            q = q.Where(c => c.Server == server);

        if (!string.IsNullOrWhiteSpace(cls) && Enum.TryParse<CharacterClass>(cls, out var parsedClass))
            q = q.Where(c => c.Class == parsedClass);

        totalCount = q.Count();
        return q.OrderByDescending(c => c.Level)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();
    }

    public Character? GetCharacter(int id) =>
        _db.Characters.Include(c => c.Inventory).FirstOrDefault(c => c.Id == id);
}
