using DataBase.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OfficeMonitor.DTOs;
using OfficeMonitor.ErrorHandler.Errors;
using OfficeMonitor.Models;
using OfficeMonitor.Models.Departments;
using OfficeMonitor.Models.Manager;
using OfficeMonitor.Services.MasterService;
using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.Design;
using System.Diagnostics;
using System.Security.Claims;

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
        /// Partial Gets Methods
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetPlans")]
        public async Task<IActionResult> GetPlans()
        {
            var plans = await ms.Plan.GetAllDtos();
            return PartialView("PartialViews/GetPlans", plans);
        }
        [Authorize(Roles = "COMPANY")]
        [HttpGet("GetCompanyInfo")]
        public async Task<IActionResult> GetCompanyInfo()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int companyId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out companyId);
            var companyDto = await ms.Company.GetDtoById(companyId);
            if (companyDto == null)
                throw new NotFoundException($"Компания не найдена. id={companyId}");
            return PartialView("PartialViews/CompanyInfo", companyDto);
        }
        [Authorize(Roles = "COMPANY")]
        [HttpGet("GetCompanyDepartmentsInfo")]
        public async Task<IActionResult> GetCompanyDepartmentsInfo()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int companyId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out companyId);
            var companyDto = await ms.Company.GetDtoById(companyId);
            if (companyDto == null)
                throw new NotFoundException($"Компания не найдена. id={companyId}");

            var departments = (await ms.Department.GetAllDtos()).Where(x => x != null && x.IdCompany != null
                                                                            && x.IdCompany.Equals(companyId));
            List<GetDepartmentModel> departmentModels = new List<GetDepartmentModel>();
            foreach (var department in departments)
            {
                int countOfEmployees = 0;
                foreach (Employee employee in await ms.Employee.GetAll())
                {
                    if (employee.IdProfile != null && await ms.IsProfileExistsInDepartment(employee.IdProfile.Value, department.Id))
                        countOfEmployees++;
                }
                //var countOfManagers1 = (await ms.Manager.GetAll()).Where(async x => x != null && x.IdProfile != null && await ms.IsProfileExistsInDepartment(x.IdProfile.Value, department.Id));
                int countOfManagers = 0;
                foreach (Manager manager in await ms.Manager.GetAll())
                {
                    if (manager.IdProfile != null && await ms.IsProfileExistsInDepartment(manager.IdProfile.Value, department.Id))
                        countOfManagers++;
                }
                int countOfManagerForDepartment = (await ms.DepartmentManager.GetAll()).Where(x => x != null && x.IdDepartment != null
                                                                                                    && x.IdDepartment.Equals(department.Id)).Count();
                departmentModels.Add(new GetDepartmentModel
                {
                    Id = department.Id,
                    Name = department.Name,
                    Description = department.Description,
                    IdCompany = companyId,
                    CountOfWorkers = countOfEmployees + countOfManagers,
                    CountOfManagers = countOfManagerForDepartment
                });
            }
            return PartialView("PartialViews/Departments", departmentModels);
        }
        [Authorize(Roles = "COMPANY")]
        [HttpGet("GetCompanyManagersInfo")]
        public async Task<IActionResult> GetCompanyManagersInfo()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int companyId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out companyId);
            var companyDto = await ms.Company.GetDtoById(companyId);
            if (companyDto == null)
                throw new NotFoundException($"Компания не найдена. id={companyId}");

            List<GetManagerModel> managerModels = new List<GetManagerModel>();
            foreach (Manager manager in await ms.Manager.GetAll())
            {
                ProfileDto profileDto = null;
                DepartmentDto departmentDto = null;
                if (manager.IdProfile != null)
                {
                    profileDto = await ms.Profile.GetDtoById(manager.IdProfile.Value);
                    if (profileDto != null)
                        departmentDto = await ms.Department.GetDtoById(profileDto.IdDepartment.Value);
                }

                managerModels.Add(new GetManagerModel
                {
                    Id = manager.Id,
                    Name = manager.Name,
                    Surname = manager.Surname,
                    Patronamic = manager.Patronamic,
                    Login = manager.Login,
                    Password = manager.Password,
                    Department = departmentDto,
                    Profile = profileDto
                });
            }
            return PartialView("PartialViews/Managers", managerModels);
        }
        [Authorize(Roles = "MANAGER")]
        [HttpGet("GetManagerInfo")]
        public async Task<IActionResult> GetManagerInfo()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int managerId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out managerId);
            var managerDto = await ms.Manager.GetDtoById(managerId);
            if (managerDto == null)
                throw new NotFoundException($"Менеджер не найден. id={managerId}");

            ProfileDto profileDto = null;
            DepartmentDto departmentDto = null;
            if (managerDto.IdProfile != null)
            {
                profileDto = await ms.Profile.GetDtoById(managerDto.IdProfile.Value);
                if (profileDto != null)
                    departmentDto = await ms.Department.GetDtoById(profileDto.IdDepartment.Value);
            }

            GetManagerModel managerModel =  new GetManagerModel
            {
                Id = managerDto.Id,
                Name = managerDto.Name,
                Surname = managerDto.Surname,
                Patronamic = managerDto.Patronamic,
                Login = managerDto.Login,
                Password = managerDto.Password,
                Department = departmentDto,
                Profile = profileDto
            };

            return PartialView("Partialviews/ManagerInfo", managerModel);
        }
        [Authorize(Roles = "MANAGER")]
        [HttpGet("GetManagerDepartments")]
        public async Task<IActionResult> GetManagerDepartments()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int managerId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out managerId);
            var managerDto = await ms.Manager.GetDtoById(managerId);
            if (managerDto == null)
                throw new NotFoundException($"Менеджер не найден. id={managerId}");

            var departmentsManager = (await ms.DepartmentManager.GetAll()).Where(x=>x!=null && x.IdManager != null && x.IdDepartment!=null
                                                                                            && x.IdManager.Equals(managerId));

            var departments = (await ms.Department.GetAllDtos()).Where(x => x != null && 
                                                                            departmentsManager.Any(a=>a.IdManager!= null && a.IdManager.Equals(managerId) &&
                                                                                                      a.IdDepartment != null && a.IdDepartment.Equals(x.Id)));
            List<GetDepartmentModel> departmentModels = new List<GetDepartmentModel>();
            foreach (var department in departments)
            {
                int countOfEmployees = 0;
                foreach (Employee employee in await ms.Employee.GetAll())
                {
                    if (employee.IdProfile != null && await ms.IsProfileExistsInDepartment(employee.IdProfile.Value, department.Id))
                        countOfEmployees++;
                }
                //var countOfManagers1 = (await ms.Manager.GetAll()).Where(async x => x != null && x.IdProfile != null && await ms.IsProfileExistsInDepartment(x.IdProfile.Value, department.Id));
                int countOfManagers = 0;
                foreach (Manager manager in await ms.Manager.GetAll())
                {
                    if (manager.IdProfile != null && await ms.IsProfileExistsInDepartment(manager.IdProfile.Value, department.Id))
                        countOfManagers++;
                }
                int countOfManagerForDepartment = (await ms.DepartmentManager.GetAll()).Where(x => x != null && x.IdDepartment != null
                                                                                                    && x.IdDepartment.Equals(department.Id)).Count();
                departmentModels.Add(new GetDepartmentModel
                {
                    Id = department.Id,
                    Name = department.Name,
                    Description = department.Description,
                    IdCompany = department.IdCompany.Value,
                    CountOfWorkers = countOfEmployees + countOfManagers,
                    CountOfManagers = countOfManagerForDepartment
                });
            }
            return PartialView("PartialViews/Departments", departmentModels);
        }
        ///  End of Partial Gets Methods

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
