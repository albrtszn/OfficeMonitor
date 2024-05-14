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
        [HttpGet("GetCompanyPlans")]
        public async Task<IActionResult> GetCompanyPlans()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int companyId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out companyId);
            var companyDto = await ms.Company.GetDtoById(companyId);
            if (companyDto == null)
                throw new NotFoundException($"Компания не найдена. id={companyId}");
            PlanDto planDto = await ms.Plan.GetDtoById(companyDto.IdPlan.Value);
            if(planDto == null)
                throw new NotFoundException($"План не найден. id={companyDto.IdPlan}");
            List<PlanDto> plans = await ms.Plan.GetAllDtos();
            CompanyPlansModel model = new CompanyPlansModel {
                CompanyPlan = planDto,
                Plans = plans
            };
            return PartialView("PartialViews/GetCompanyPlans", model);
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
        [Authorize(Roles = "MANAGER")]
        [HttpPost("GetDepartmentEmployeesContent")]
        public async Task<IActionResult> GetDepartmentEmployeesContent([FromBody] IntIdModel? id)
        {
            if (!ModelState.IsValid)//id == null || !id.Id.HasValue || id.Id == 0)
                throw new BadRequestException("Невалидное значение");
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int managerId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out managerId);
            var managerDto = await ms.Manager.GetDtoById(managerId);
            if (managerDto == null)
                throw new NotFoundException($"Менеджер не найден. id={managerId}");

            var departmentsManager = (await ms.DepartmentManager.GetAll()).Where(x => x != null && x.IdManager != null && x.IdDepartment != null
                                                                                            && x.IdManager.Equals(managerId));

            var department = (await ms.Department.GetAllDtos()).Find(x => x != null &&
                                                                            departmentsManager.Any(a => a.IdManager != null && a.IdManager.Equals(managerId) &&
                                                                                                        a.IdDepartment != null && a.IdDepartment.Equals(x.Id)));
            List<GetEmployeeModel> models = new List<GetEmployeeModel>();
            foreach (Employee employee in await ms.Employee.GetAll())
            {
                if (employee.IdProfile != null && await ms.IsProfileExistsInDepartment(employee.IdProfile.Value, department.Id))
                {
                    ProfileDto profileDto = await ms.Profile.GetDtoById(employee.IdProfile.Value);
                    models.Add(new GetEmployeeModel
                    {
                        Id = employee.Id,
                        Name = employee.Name,
                        Surname = employee.Surname,
                        Patronamic = employee.Patronamic,
                        Login = employee.Login,
                        Password = employee.Password,
                        Department = await ms.Department.GetDtoById(profileDto.IdDepartment.Value),
                        Profile = profileDto
                    });
                }
            }
            return PartialView("PartialViews/Modal/DepartmentEmployeesContent", models);
        }
        [Authorize(Roles = "MANAGER")]
        [HttpGet("AddEmployeeContent")]
        public async Task<IActionResult> AddEmployeeContent()
        {
            if (!ModelState.IsValid)//id == null || !id.Id.HasValue || id.Id == 0)
                throw new BadRequestException("Невалидное значение");
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int managerId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out managerId);
            var managerDto = await ms.Manager.GetDtoById(managerId);
            if (managerDto == null)
                throw new NotFoundException($"Менеджер не найден. id={managerId}");

            return PartialView("PartialViews/Modal/AddEmployeeContent");
        }        
        [Authorize(Roles = "MANAGER")]
        [HttpPost("UpdateEmployeeContent")]
        public async Task<IActionResult> UpdateEmployeeContent([FromBody] IntIdModel? id)
        {
            if (!ModelState.IsValid)//id == null || !id.Id.HasValue || id.Id == 0)
                throw new BadRequestException("Невалидное значение");
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int managerId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out managerId);
            var managerDto = await ms.Manager.GetDtoById(managerId);
            if (managerDto == null)
                throw new NotFoundException($"Менеджер не найден. id={managerId}");

            GetEmployeeModel model = await ms.GetEmployeeModelbyId(id.Id);
            if(model == null)
                throw new BadRequestException("Пользователь не найден");
            return PartialView("PartialViews/Modal/UpdateEmployeeContent", model);
        }

        [Authorize(Roles = "MANAGER")]
        [HttpGet("GetWorkTimeOfDepartments")]
        public async Task<IActionResult> GetWorkTimeOfDepartments()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int managerId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out managerId);
            var managerDto = await ms.Manager.GetDtoById(managerId);
            if (managerDto == null)
                throw new NotFoundException($"Менеджер не найден. id={managerId}");

            var departmentsManager = (await ms.DepartmentManager.GetAll()).Where(x => x != null && x.IdManager != null && x.IdDepartment != null
                                                                                            && x.IdManager.Equals(managerId));

            var departmentDtos = (await ms.Department.GetAllDtos()).Where(x => x != null &&
                                                                            departmentsManager.Any(a => a.IdManager != null && a.IdManager.Equals(managerId) &&
                                                                                                      a.IdDepartment != null && a.IdDepartment.Equals(x.Id)));
            List<GetWorkTimeModel> models = new List<GetWorkTimeModel>();
            foreach (var department in departmentDtos)
            {
                WorkTimeDto? workTimeDto = await ms.WorkTime.GetDtoByDepartmentId(department.Id);
                if(workTimeDto == null)
                {
                    models.Add(new GetWorkTimeModel
                    {
                        Id = 0,
                        Department = department,
                        StartTime = TimeOnly.MinValue,
                        EndTime = TimeOnly.MinValue,
                    });
                }
                else
                {
                    models.Add(new GetWorkTimeModel
                    {
                        Id = workTimeDto.Id,
                        Department = department,
                        StartTime = workTimeDto.StartTime.Value,
                        EndTime = workTimeDto.EndTime.Value
                    });
                }
            }
            return PartialView("PartialViews/GetWorkTimeOfDepartments", models);
        }
        [Authorize(Roles = "MANAGER")]
        [HttpPost("AddWorkTimeContent")]
        public async Task<IActionResult> AddWorkTimeContent([FromBody]IntIdModel id)
        {
            if (!ModelState.IsValid)//id == null || !id.Id.HasValue || id.Id == 0)
                throw new BadRequestException("Невалидное значение");
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int managerId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out managerId);
            DepartmentDto departmentDto = await ms.Department.GetDtoById(id.Id);
            if (departmentDto == null)
                throw new NotFoundException($"Запись не найдена. id={id.Id}");

            return PartialView("PartialViews/Modal/WorkTime/AddWorkTimeContent", departmentDto);
        }
        [Authorize(Roles = "MANAGER")]
        [HttpPost("UpdateWorkTimeContent")]
        public async Task<IActionResult> UpdateWorkTimeContent([FromBody] IntIdModel? id)
        {
            if (!ModelState.IsValid)//id == null || !id.Id.HasValue || id.Id == 0)
                throw new BadRequestException("Невалидное значение");
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int managerId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out managerId);
            GetWorkTimeModel model = await ms.GetWorkTimeModel(id.Id);
            if (model == null)
                throw new NotFoundException($"Запись не найдена. id={id.Id}");
            return PartialView("PartialViews/Modal/WorkTime/UpdateWorkTimeContent", model);
        }

        [Authorize(Roles = "MANAGER")]
        [HttpGet("GetDepartmentStatistics")]
        public async Task<IActionResult> GetDepartmentStatistics()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int managerId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out managerId);
            var managerDto = await ms.Manager.GetDtoById(managerId);
            if (managerDto == null)
                throw new NotFoundException($"Менеджер не найден. id={managerId}");

            var departmentsManager = (await ms.DepartmentManager.GetAll()).Where(x => x != null && x.IdManager != null && x.IdDepartment != null
                                                                                            && x.IdManager.Equals(managerId));

            var departmentDtos = (await ms.Department.GetAllDtos()).Where(x => x != null &&
                                                                            departmentsManager.Any(a => a.IdManager != null && a.IdManager.Equals(managerId) &&
                                                                                                      a.IdDepartment != null && a.IdDepartment.Equals(x.Id)))
                                                                    .ToList();
            return PartialView("PartialViews/DepartmentsStatistic", departmentDtos);
        }

        [Authorize(Roles = "MANAGER")]
        [HttpPost("GetDepartmentStatistic")]
        public async Task<IActionResult> GetDepartmentStatistic([FromBody] GetDepartmentStatistic model)
        {
            if (!ModelState.IsValid)//id == null || !id.Id.HasValue || id.Id == 0)
                throw new BadRequestException("Невалидное значение");
            logger.LogInformation($"/api/GetDepartmentStatistic POST departmentId={model.DepartmentId}, dataRane={model.DateRange}");

            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int managerId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out managerId);
            var managerDto = await ms.Manager.GetDtoById(managerId);
            if (managerDto == null)
                throw new NotFoundException($"Менеджер не найден. id={managerId}");

            var departmentsManager = (await ms.DepartmentManager.GetAll()).Where(x => x != null && x.IdManager != null && x.IdDepartment != null
                                                                                           && x.IdManager.Equals(managerId));

            var departmentDto = (await ms.Department.GetAllDtos()).Find(x => x != null && x.Id.Equals(model.DepartmentId) &&
                                                                             departmentsManager.Any(a=> a.IdDepartment != null && a.IdDepartment.Equals(model.DepartmentId)));

            DepartmentStatistic statistic = new DepartmentStatistic();
            List<EmployeeDto> employees = await ms.GetEmployeeDtosByDepartment(model.DepartmentId);
            WorkTimeDto? workTimeOfDepartment = await ms.WorkTime.GetDtoByDepartmentId(model.DepartmentId);
            if (departmentDto != null && workTimeOfDepartment != null && !employees.IsNullOrEmpty())
            { 
                int totalWorkedHours = 0;
                int totalIdleHours = 0;
                int totalDiverHours = 0;                        
                string[] dates = model.DateRange.Split(" - ");
                DateOnly startDate;
                DateOnly.TryParse(dates[0], out startDate);
                DateOnly endDate;
                DateOnly.TryParse(dates[1], out endDate);
                foreach (var employee in employees)
                {
                    List<Action> employeeActions = await ms.Action.GetAllByEmployee(employee.Id);
                    foreach(var action in employeeActions)
                    {
                        if (action.Date != null && 
                            (action.Date.Value>=startDate && action.Date.Value<=endDate))
                        {

                        }
                    }
                }

                double totalMinutes = 0;
                double minutesPerEmployee = (workTimeOfDepartment.EndTime - workTimeOfDepartment.StartTime).Value.TotalMinutes;
                while (startDate<=endDate)
                {
                    if (!startDate.DayOfWeek.Equals(DayOfWeek.Saturday) && !startDate.DayOfWeek.Equals(DayOfWeek.Sunday))
                        totalMinutes += minutesPerEmployee;
                    startDate = startDate.AddDays(1);
                }

                statistic.Name = departmentDto.Name;
                statistic.WorkTime = workTimeOfDepartment;
                statistic.WorkedPercent = 0;
                statistic.IdlePercent = 0;
                statistic.DiversionPercent = 0;
                statistic.TotalHours = new TimeSummaryModel
                { 
                    Hours = ((int)totalMinutes / 60).ToString(),
                    Minutes = ((int)totalMinutes % 60).ToString()
                };
            }
            return PartialView("PartialViews/GetDepartmentStatistic", statistic);
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
