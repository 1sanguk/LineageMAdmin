using LineageMOps.Models.Domain;

namespace LineageMOps.Models.ViewModels;

public class CharacterDetailViewModel
{
    public Character Character { get; set; } = null!;
    public Account? Account { get; set; }
    public List<Item> AllItems { get; set; } = new();
}
