using LineageMOps.Data;
using LineageMOps.Models.Domain;

namespace LineageMOps.Services;

public class GameDataService : IGameDataService
{
    private readonly MockDataStore _store;

    public GameDataService(MockDataStore store) => _store = store;

    public List<Character> SearchCharacters(string? query, string? server, string? cls, int page, int pageSize, out int totalCount)
    {
        var q = _store.Characters.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(query))
            q = q.Where(c => c.Name.Contains(query, StringComparison.OrdinalIgnoreCase));

        if (!string.IsNullOrWhiteSpace(server))
            q = q.Where(c => c.Server == server);

        if (!string.IsNullOrWhiteSpace(cls) && Enum.TryParse<CharacterClass>(cls, out var parsedClass))
            q = q.Where(c => c.Class == parsedClass);

        var list = q.OrderByDescending(c => c.Level).ToList();
        totalCount = list.Count;
        return list.Skip((page - 1) * pageSize).Take(pageSize).ToList();
    }

    public Character? GetCharacter(int id) => _store.Characters.FirstOrDefault(c => c.Id == id);
}
