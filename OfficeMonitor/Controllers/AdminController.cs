using Microsoft.AspNetCore.Mvc;

namespace OfficeMonitor.Controllers
{
    [Route("Admin")]
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
