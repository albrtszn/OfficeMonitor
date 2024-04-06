using Microsoft.AspNetCore.Mvc;
using OfficeMonitor.Models;
using OfficeMonitor.Services.MasterService;
using System.Diagnostics;

namespace OfficeMonitor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private MasterService ms;

        public HomeController(ILogger<HomeController> _logger, MasterService _ms)
        {
            logger = _logger;
            ms = _ms;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
