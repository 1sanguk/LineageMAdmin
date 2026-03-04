using LineageMOps.Models.Domain;

namespace LineageMOps.Services;

public interface IGameDataService
{
    List<Character> SearchCharacters(string? query, string? server, string? cls, int page, int pageSize, out int totalCount);
    Character? GetCharacter(int id);
}
