using DataBase.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeMonitor.ErrorHandler.Errors;
using OfficeMonitor.Models;
using OfficeMonitor.Services.MasterService;
using Swashbuckle.AspNetCore.Annotations;
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
        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginModel model)
        {
            if (model == null || !ModelState.IsValid)
            {
                throw new BadRequestException("Невалидное значение");
            }
            //  todo create Token in DB
            string? token = null;
            Employee? employee = await ms.Employee.GetByEmail(model.Login);
            Manager? manager = await ms.Manager.GetByEmail(model.Login);
            Admin? admin = await ms.Admin.GetByEmail(model.Login);
            Company? company = await ms.Company.GetByEmail(model.Login);
            if (employee != null)
            {
                token = await ms.Employee.Login(model.Login, model.Password);
            }
            if (manager != null)
            {
                token = await ms.Manager.Login(model.Login, model.Password);
            }
            if (admin != null)
            {
                token = await ms.Admin.Login(model.Login, model.Password);
            }
            if (company != null)
            {
                token = await ms.Company.Login(model.Login, model.Password);
            }
            if (token == null)
                throw new NotFoundException("Неправильные данные");

            logger.LogInformation($"/Login POST login={model.Login}, password={model.Password}; token={token}");

            // todo cookie numberations
            HttpContext.Response.Cookies.Append("cookie#1", token);

            return RedirectToAction("Index");
        }

        [Authorize(Roles = "USER")]
        public async Task<IActionResult> TestUser()
        {
            return Ok("Success, user");
        }
        [Authorize(Roles = "MANAGER")]
        public async Task<IActionResult> TestManager()
        {
            return Ok("Success, manager");
        }
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> TestAdmin()
        {
            return Ok("Success, admin");
        }
        [Authorize(Roles = "COMPANY")]
        public async Task<IActionResult> TestCompany()
        {
            return Ok("Success, company");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
