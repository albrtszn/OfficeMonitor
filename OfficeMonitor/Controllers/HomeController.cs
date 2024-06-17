using DataBase.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using OfficeMonitor.DTOs;
using OfficeMonitor.ErrorHandler.Errors;
using OfficeMonitor.Models;
using OfficeMonitor.Models.Company;
using OfficeMonitor.Models.Departments;
using OfficeMonitor.Models.Employee;
using OfficeMonitor.Models.Manager;
using OfficeMonitor.Models.Request;
using OfficeMonitor.Models.WorkTime;
using OfficeMonitor.Services.MasterService;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Security.Claims;
using Action = DataBase.Repository.Models.Action;

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
        //  todo page get
        /// <summary>
        /// Partial View Methods
        /// </summary>
        [HttpGet("GetPlans")]
        public async Task<IActionResult> GetPlans()
        {
            var plans = await ms.Plan.GetAllDtos();
            return PartialView("PartialViews/GetPlans", plans);
        }

        [HttpGet("AdminDashboard")]
        public async Task<IActionResult> AdminDashboard()
        {
            return View("AdminDashboard");
        }
        [HttpGet("GetAdminInfo")]
        public async Task<IActionResult> GetAdminInfo()
        {
            return PartialView("PartialViews/GetAdminInfo");
        }
        [HttpGet("GetCompanies")]
        public async Task<IActionResult> GetCompanies()
        {
            var companies = await ms.Company.GetAllDtos();
            return PartialView("PartialViews/GetCompanies", companies);
        }
        [HttpGet("GetAdminPlans")]
        public async Task<IActionResult> GetAdminPlans()
        {
            var plans = await ms.Plan.GetAllDtos();
            return PartialView("PartialViews/GetAdminPlans", plans);
        }
        ///  End of Partial Gets Methods
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet("Privacy")]
        public IActionResult Privacy()
        {
            return View();
        }
        [HttpGet("CustomerRequest")]
        public IActionResult CustomerRequest()
        {
            return PartialView("Modal/AddCustomerRequestContent");
        }
        [HttpPost("AddCustomerRequest")]
        public async Task<IActionResult> AddCustomerRequest([FromForm] AddCustomerRequestModel model)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            if(await ms.Plan.GetById(model.IdPlan) == null)
                throw new NotFoundException("Значение не нейдено");

            if(await ms.CustomerRequest.GetByEmail(model.Email) != null)
                throw new BadRequestException("Значение уже есть");
            await ms.CustomerRequest.Save(new CustomerRequest
            {
                Email = model.Email,
                Name = model.Name,
                IsReplyed = true
            });

            if (await ms.Company.GetByEmail(model.Email) != null)
                throw new BadRequestException("Значение уже есть");
            ClaimRole? claimRole = (await ms.ClaimRole.GetCompanyRole());
            await ms.Company.Save(new Company
            {
                IdClaimRole = claimRole.Id,
                Login = model.Email,
                Password = model.Password,
                Name = model.Name,
                Description = model.Description,
                IdPlan = model.IdPlan,
                Balance = 0,
                IsActive = true,
                IsBanned = false,
                DateOfRegister = DateTime.Now,
                DateOfEndPayment = DateTime.Now
            });
            return Ok();
        }
        [HttpGet("LogOut")]
        public IActionResult LogOut()
        {
            int countOfCookies = 0;
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
                countOfCookies++;
            }
            return Redirect("/");
        }
        [HttpGet("Login")]
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
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value;
            var role = claimsIdentity.FindFirst(x => x.Type.Contains("role"))?.Value;
            return Ok($"Success, user. EmployeeId={userId}, Role={role}");
        }
        [Authorize(Roles = "MANAGER")]
        public async Task<IActionResult> TestManager()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value;
            var role = claimsIdentity.FindFirst(x => x.Type.Contains("role"))?.Value;
            return Ok($"Success, manager. ManagerId={userId}, Role={role}");
        }
        [Authorize(Roles = "ADMIN")]
        public async Task<IActionResult> TestAdmin()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value;
            var role = claimsIdentity.FindFirst(x => x.Type.Contains("role"))?.Value;
            return Ok($"Success, admin. AdminId={userId}, Role={role}");
        }
        [Authorize(Roles = "COMPANY")]
        public async Task<IActionResult> TestCompany()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            var userId = claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value;
            var role = claimsIdentity.FindFirst(x => x.Type.Contains("role"))?.Value;
            return Ok($"Success, company. CompanyId={userId}, Role={role}");
        }
        [Authorize(Roles = "MANAGER")]
        [HttpGet("ManagerDashboard")]
        public IActionResult ManagerDashboard()
        {
            return View();
        }
        [Authorize(Roles = "COMPANY")]
        [HttpGet("CompanyDashboard")]
        public IActionResult CompanyDashboard()
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
