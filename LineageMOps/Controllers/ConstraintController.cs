using LineageMOps.Models.Domain;
using LineageMOps.Services;
using Microsoft.AspNetCore.Mvc;

namespace LineageMOps.Controllers;

public class ConstraintController : Controller
{
    private readonly IConstraintService _constraints;
    private readonly IAdminLogService _adminLogs;

    public ConstraintController(IConstraintService constraints, IAdminLogService adminLogs)
    {
        _constraints = constraints;
        _adminLogs = adminLogs;
    }

    public IActionResult Index()
    {
        ViewData["Title"] = "게임 Constraint 관리";
        var groups = _constraints.GetAll()
            .GroupBy(c => c.Category)
            .ToDictionary(g => g.Key, g => g.ToList());
        return View(groups);
    }

    [HttpPost]
    public IActionResult Update(int id, string value)
    {
        var constraint = _constraints.GetById(id);
        if (constraint is null)
        {
            TempData["Error"] = "존재하지 않는 Constraint입니다.";
            return RedirectToAction(nameof(Index));
        }

        if (!IsValueValid(constraint, value))
        {
            TempData["Error"] = $"[{constraint.DisplayName}] 유효하지 않은 값입니다.";
            return RedirectToAction(nameof(Index));
        }

        var oldValue = constraint.Value;
        _constraints.UpdateValue(id, value);
        _adminLogs.Add("Constraint 수정", constraint.Key, $"{constraint.DisplayName}: {oldValue} → {value}");

        TempData["Success"] = $"[{constraint.DisplayName}] 값이 {value}(으)로 변경되었습니다.";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost]
    public IActionResult Reset(int id)
    {
        var constraint = _constraints.GetById(id);
        if (constraint is null)
        {
            TempData["Error"] = "존재하지 않는 Constraint입니다.";
            return RedirectToAction(nameof(Index));
        }

        var oldValue = constraint.Value;
        _constraints.ResetToDefault(id);
        _adminLogs.Add("Constraint 초기화", constraint.Key, $"{constraint.DisplayName}: {oldValue} → {constraint.DefaultValue} (기본값 복원)");

        TempData["Success"] = $"[{constraint.DisplayName}] 기본값({constraint.DefaultValue})으로 복원되었습니다.";
        return RedirectToAction(nameof(Index));
    }

    private static bool IsValueValid(GameConstraint c, string value)
    {
        return c.ValueType switch
        {
            ConstraintValueType.Bool  => value == "true" || value == "false",
            ConstraintValueType.Int   => int.TryParse(value, out var i)
                                         && (c.Min is null || i >= c.Min)
                                         && (c.Max is null || i <= c.Max),
            ConstraintValueType.Float => double.TryParse(value, System.Globalization.NumberStyles.Any,
                                             System.Globalization.CultureInfo.InvariantCulture, out var f)
                                         && (c.Min is null || f >= c.Min)
                                         && (c.Max is null || f <= c.Max),
            _ => false
        };
    }
}
