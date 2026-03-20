using LineageMOps.Models.Domain;

namespace LineageMOps.Services;

public interface IConstraintService
{
    List<GameConstraint> GetAll();
    GameConstraint? GetById(int id);
    void UpdateValue(int id, string value);
    void ResetToDefault(int id);
}
