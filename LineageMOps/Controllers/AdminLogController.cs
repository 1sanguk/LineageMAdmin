using LineageMOps.Services;
using Microsoft.AspNetCore.Mvc;

namespace LineageMOps.Controllers;

public class AdminLogController : Controller
{
    private readonly IAdminLogService _adminLog;

    public AdminLogController(IAdminLogService adminLog) => _adminLog = adminLog;

    public IActionResult Index()
    {
        ViewData["Title"] = "어드민 로그";
        var logs = _adminLog.GetRecent(100);
        return View(logs);
    }
}
