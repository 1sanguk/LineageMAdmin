using LineageMOps.Data;
using LineageMOps.Models.Domain;

namespace LineageMOps.Services;

public class ConstraintService : IConstraintService
{
    private readonly MockDataStore _store;

    public ConstraintService(MockDataStore store) => _store = store;

    public List<GameConstraint> GetAll() => _store.Constraints;

    public GameConstraint? GetById(int id) =>
        _store.Constraints.FirstOrDefault(c => c.Id == id);

    public void UpdateValue(int id, string value)
    {
        var constraint = _store.Constraints.FirstOrDefault(c => c.Id == id);
        if (constraint is null) return;
        constraint.Value = value;
        constraint.UpdatedAt = DateTime.Now;
    }

    public void ResetToDefault(int id)
    {
        var constraint = _store.Constraints.FirstOrDefault(c => c.Id == id);
        if (constraint is null) return;
        constraint.Value = constraint.DefaultValue;
        constraint.UpdatedAt = DateTime.Now;
    }
}
