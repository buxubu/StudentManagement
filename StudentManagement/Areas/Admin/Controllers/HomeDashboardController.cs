using Microsoft.AspNetCore.Mvc;

namespace StudentManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeDashboardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
