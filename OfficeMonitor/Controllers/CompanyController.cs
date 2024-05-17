using DataBase.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using OfficeMonitor.DTOs;
using OfficeMonitor.ErrorHandler.Errors;
using OfficeMonitor.Models.Departments;
using OfficeMonitor.Models.Employee;
using OfficeMonitor.Models.Manager;
using OfficeMonitor.Models.WorkTime;
using OfficeMonitor.Models;
using System.Security.Claims;
using OfficeMonitor.Services.MasterService;
using Action = DataBase.Repository.Models.Action;
using OfficeMonitor.Models.Company;

namespace OfficeMonitor.Controllers
{
    [Route("Company")]
    public class CompanyController : Controller
    {
        private readonly ILogger<CompanyController> logger;
        private MasterService ms;

        public CompanyController(ILogger<CompanyController> _logger, MasterService _ms)
        {
            logger = _logger;
            ms = _ms;
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
            if (planDto == null)
                throw new NotFoundException($"План не найден. id={companyDto.IdPlan}");
            List<PlanDto> plans = await ms.Plan.GetAllDtos();
            CompanyPlansModel model = new CompanyPlansModel
            {
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
        [HttpGet("GetDepartments")]
        public async Task<IActionResult> GetDepartments()
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
        [HttpGet("GetManagers")]
        public async Task<IActionResult> GetManagers()
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
    }
}
