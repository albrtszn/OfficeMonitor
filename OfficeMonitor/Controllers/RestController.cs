using AutoMapper;
using DataBase.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OfficeMonitor.DTOs;
using OfficeMonitor.ErrorHandler.Errors;
using OfficeMonitor.Models;
using OfficeMonitor.Models.ClaimRole;
using OfficeMonitor.Models.Company;
using OfficeMonitor.Models.Departments;
using OfficeMonitor.Models.Employee;
using OfficeMonitor.Models.Manager;
using OfficeMonitor.Models.WorkTime;
using OfficeMonitor.Services;
using OfficeMonitor.Services.MasterService;
using Swashbuckle.AspNetCore.Annotations;
using System.Security.Claims;
using System.Text.Json.Serialization;
using Profile = DataBase.Repository.Models.Profile;
using Action = DataBase.Repository.Models.Action;
using OfficeMonitor.Models.Action;
using OfficeMonitor.Models.App;
using OfficeMonitor.Models.TypeApp;
using OfficeMonitor.Models.Profile;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OfficeMonitor.Controllers
{
    //[Route("api/[controller]")]
    [Route("api")]
    [ApiController]
    public class RestController : ControllerBase
    {
        private readonly ILogger<HomeController> logger;
        private IMapper mapper;
        private MasterService ms;

        public RestController(ILogger<HomeController> _logger, IMapper _mapper,
                              MasterService _ms)
        {
            logger = _logger;
            mapper = _mapper;
            ms = _ms;
        }

        [HttpGet("Ping")]
        public async Task<IActionResult> Ping()
        {
            return Ok(new { message = $"Ping: {HttpContext.Request.Host + HttpContext.Request.Path} {DateTime.Now}." });
        }


        [HttpPost("PostPing")]
        public async Task<IActionResult> PostPing()
        {
            return Ok(new { message = $"Ping: {HttpContext.Request.Host + HttpContext.Request.Path} {DateTime.Now}." });
        }

        [HttpGet("TestMethod")]
        public IActionResult TestMethod()
        {
            Employee empl = new Employee
            {
                Id = 1,
                Name = "Test1",
                Surname = "Test1",
                Patronamic = "Test1",
                Login = "login",
                Password = "password",
                IdProfile = 1
            };
            var emplDto = mapper.Map<EmployeeDto>(empl);

            return Ok(new { message = $"{DateTime.Now} Employee type: {emplDto.GetType()} .", emplDto });
        }

        [HttpGet("ClearCookies")]
        public IActionResult ClearCookies()
        {
            int countOfCookies = 0;
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
                countOfCookies++;
            }
            return Ok(new { message = $"{DateTime.Now} Deleted {countOfCookies} cookies"});
        }

        /// <summary>
        /// Login through email
        /// </summary>
        /// <param name="model"></param>
        /// <returns>Status, token</returns>
        /// <remarks>
        /// Sample request:
        /// 
        ///  POST /Login
        ///     {
        ///        "Login": "employee@gmail.com",
        ///        "Password": "password"
        ///     }
        ///     
        /// </remarks>
        /// <response code="201">Returns the newly token</response>
        /// <response code="400">If the LoginModel is incorrect</response>
        /// <response code="404">If the user not found</response>
        [HttpPost("Login")]
        public async  Task<IActionResult> Login(LoginModel model)
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
            if(employee != null)
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

            // todo cookie numberations
            HttpContext.Response.Cookies.Append("cookie#1", token);

            return Ok(new { message = $"login succesful. token:{token}" });
        }

        [Authorize(Roles = "USER")]
        [SwaggerOperation(Tags = new[] { "Rest/TestAuthorization" })]
        [HttpGet("TestUser")]
        public async Task<IActionResult> TestUser()
        {
            return Ok("Success, user");
        }
        [Authorize(Roles = "MANAGER")]
        [SwaggerOperation(Tags = new[] { "Rest/TestAuthorization" })]
        [HttpGet("TestManager")]
        public async Task<IActionResult> TestManager()
        {
            return Ok("Success, manager");
        }
        [Authorize(Roles = "ADMIN")]
        [SwaggerOperation(Tags = new[] { "Rest/TestAuthorization" })]
        [HttpGet("TestAdmin")]
        public async Task<IActionResult> TestAdmin()
        {
            return Ok("Success, admin");
        }
        [Authorize(Roles = "COMPANY")]
        [SwaggerOperation(Tags = new[] { "Rest/TestAuthorization" })]
        [HttpGet("TestCompany")]
        public async Task<IActionResult> TestCompany()
        {
            return Ok("Success, company");
        }

        /*
        *  #Tokens
        */
        [SwaggerOperation(Tags = new[] { "Rest/Token" })]
        [HttpGet("GetEmployeeTokens")]
        public async Task<IActionResult> GetEmployeeTokens()
        {
            return Ok(await ms.TokenEmployee.GetAllDtos());
        }
        [SwaggerOperation(Tags = new[] { "Rest/Token" })]
        [HttpGet("GetManagerTokens")]
        public async Task<IActionResult> GetManagerTokens()
        {
            return Ok(await ms.TokenManager.GetAllDtos());
        }
        [SwaggerOperation(Tags = new[] { "Rest/Token" })]
        [HttpGet("GetAdminTokens")]
        public async Task<IActionResult> GetAdminTokens()
        {
            return Ok(await ms.TokenAdmin.GetAllDtos());
        }
        [SwaggerOperation(Tags = new[] { "Rest/Token" })]
        [HttpGet("GetCompanyTokens")]
        public async Task<IActionResult> GetCompanyTokens()
        {
            return Ok(await ms.TokenCompany.GetAllDtos());
        }

        /*
        *  #ClaimRole
        */
        [SwaggerOperation(Tags = new[] { "Rest/ClaimRole" })]
        [HttpGet("GetClaimRoles")]
        public async Task<IActionResult> GetClaimRoles()
        {
            return Ok(await ms.ClaimRole.GetAllDtos());
        }

        [SwaggerOperation(Tags = new[] { "Rest/ClaimRole" })]
        [HttpPost("GetClaimRole")]
        public async Task<IActionResult> GetClaimRole([FromBody] IntIdModel? id)
        {
            //  todo return exception with required field
            if (id == null || id.Id == 0 || !ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            ClaimRoleDto? dto = await ms.ClaimRole.GetDtoById(id.Id);
            if (dto == null)
                throw new NotFoundException("Запись не найдена");
            return Ok(dto);
        }

        [SwaggerOperation(Tags = new[] { "Rest/ClaimRole" })]
        [HttpPost("AddClaimRole")]
        public async Task<IActionResult> AddClaimRole([FromBody] AddClaimRoleModel? model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest("Невалидное значение");
            await ms.ClaimRole.Save(model);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/ClaimRole" })]
        [HttpPost("UpdateClaimRole")]
        public async Task<IActionResult> UpdateClaimRole([FromBody] UpdateClaimRoleModel? model)
        {
            if (model == null || model.Id <= 0 || !ModelState.IsValid)
                return BadRequest("Невалидное значение");
            ClaimRole? claimRole = await ms.ClaimRole.GetById(model.Id);
            if (claimRole == null)
                throw new NotFoundException("Запись не найдена");
            await ms.ClaimRole.Save(model);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/ClaimRole" })]
        [HttpPost("DeleteClaimRole")]
        public async Task<IActionResult> DeleteClaimRole([FromBody] IntIdModel? id)
        {
            if (!ModelState.IsValid)//id == null || !id.Id.HasValue || id.Id == 0)
                return BadRequest("Невалидное значение");
            ClaimRole? claimRole = await ms.ClaimRole.GetById(id.Id);
            if (claimRole == null)
                throw new NotFoundException("Запись не найдена");
            await ms.ClaimRole.DeleteById(id.Id);
            return Ok();
        }

        /*
        *  #Plan
        */
        [SwaggerOperation(Tags = new[] { "Rest/Plan" })]
        [HttpGet("GetPlans")]
        public async Task<IActionResult> GetPlans()
        {
            return Ok(await ms.Plan.GetAllDtos());
        }

        [SwaggerOperation(Tags = new[] { "Rest/Plan" })]
        [HttpGet("GetPlansRadioHtml")]
        public async Task<IActionResult> GetPlansRadioHtml()
        {
            string html = "";
            int number = 1;
            foreach (Plan plan in await ms.Plan.GetAll())
            {
                html += $"<input type=\"radio\" id=\"plan{number}\" value=\"{plan.Id}\" name=\"IdPlan\" />";
                html += $"<label for=\"plan{number}\">{plan.Name}-{plan.MonthCost} BYN/month</label></br>";
                number++;
            }
            return Content(html);
        }

        [SwaggerOperation(Tags = new[] { "Rest/Plan" })]
        [HttpPost("GetPlan")]
        public async Task<IActionResult> GetPlan([FromBody] IntIdModel? id)
        {
            //  todo return exception with required field
            if (id == null || id.Id == 0 || !ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            PlanDto? dto = await ms.Plan.GetDtoById(id.Id);
            if (dto == null)
                throw new NotFoundException("Запись не найдена");
            return Ok(dto);
        }

        [SwaggerOperation(Tags = new[] { "Rest/Plan" })]
        [HttpPost("AddPlan")]
        public async Task<IActionResult> AddPlan([FromBody] AddPlanModel? model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest("Невалидное значение");
            await ms.Plan.Save(model);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Plan" })]
        [HttpPost("UpdatePlan")]
        public async Task<IActionResult> UpdatePlan([FromBody] ProfileDto? dto)
        {
            if (dto == null || dto.Id <= 0 || !ModelState.IsValid)
                return BadRequest("Невалидное значение");
            Plan? plan = await ms.Plan.GetById(dto.Id);
            if (plan == null)
                throw new NotFoundException("Запись не найдена");
            await ms.Profile.Save(dto);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Plan" })]
        [HttpPost("DeletePlan")]
        public async Task<IActionResult> DeletePlan([FromBody] IntIdModel? id)
        {
            if (!ModelState.IsValid)//id == null || !id.Id.HasValue || id.Id == 0)
                return BadRequest("Невалидное значение");
            Plan? plan = await ms.Plan.GetById(id.Id);
            if (plan == null)
                throw new NotFoundException("Запись не найдена");
            await ms.Department.DeleteById(id.Id);
            return Ok();
        }

        /*
        *  #Company
        */
        [SwaggerOperation(Tags = new[] { "Rest/Company" })]
        [HttpGet("GetCompanies")]
        public async Task<IActionResult> GetCompanies()
        {
            return Ok(await ms.Company.GetAllDtos());
        }

        [SwaggerOperation(Tags = new[] { "Rest/Company" })]
        [HttpPost("GetCompany")]
        public async Task<IActionResult> GetCompany([FromBody] IntIdModel? id)
        {
            //  todo return exception with required field
            if (id == null || id.Id == 0 || !ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            CompanyDto? dto = await ms.Company.GetDtoById(id.Id);
            if (dto == null)
                throw new NotFoundException("Запись не найдена");
            return Ok(dto);
        }

        [SwaggerOperation(Tags = new[] { "Rest/Company" })]
        [HttpPost("AddCompany")]
        public async Task<IActionResult> AddCompany([FromBody] AddCompanyModel? model)
        {
            if (model == null || !ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            Plan? plan = await ms.Plan.GetById(model.IdPlan);
            if (plan == null)
                throw new NotFoundException("План не найден");
            ClaimRole? claimRole = (await ms.ClaimRole.GetCompanyRole());
            await ms.Company.Save(model, claimRole);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Company" })]
        [HttpPost("UpdateCompany")]
        public async Task<IActionResult> UpdateCompany([FromBody] UpdateCompanyModel? model)
        {
            if (model == null || model.Id <= 0 || !ModelState.IsValid)
                return BadRequest("Невалидное значение");
            Company? company = await ms.Company.GetById(model.Id);
            if (company == null)
                throw new NotFoundException("Запись не найдена");
            Plan? plan = await ms.Plan.GetById(model.IdPlan);
            if (plan == null)
                throw new NotFoundException("План не найден");
            ClaimRole? claimRole = (await ms.ClaimRole.GetCompanyRole());

            await ms.Company.Save(model, claimRole);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Company" })]
        [HttpPost("DeleteCompany")]
        public async Task<IActionResult> DeleteCompany([FromBody] IntIdModel? id)
        {
            if (!ModelState.IsValid)//id == null || !id.Id.HasValue || id.Id == 0)
                return BadRequest("Невалидное значение");
            Company? company = await ms.Company.GetById(id.Id);
            if (company == null)
                throw new NotFoundException("Запись не найдена");

            List<Department> deps = (await ms.Department.GetAll()).Where(x=>x.IdCompany!=null && x.IdCompany.Equals(company.Id)).ToList();
            foreach(Department department in deps)
            {
                List<Profile> profiles = (await ms.Profile.GetAll()).Where(x=>x.IdDepartment!= null && x.IdDepartment.Equals(company.Id)).ToList();
                foreach(Profile profile in profiles)
                {
                    List<Employee> employees = (await ms.Employee.GetAll()).Where(x => x.IdProfile != null && x.IdProfile.Equals(company.Id)).ToList();
                    List<Manager> managers = (await ms.Manager.GetAll()).Where(x => x.IdProfile != null && x.IdProfile.Equals(company.Id)).ToList();
                    foreach(Employee employee in employees)
                    {
                        await ms.Employee.DeleteById(employee.Id);
                    }
                    foreach (Manager manager in managers)
                    {
                        await ms.Manager.DeleteById(manager.Id);
                    }
                    await ms.Profile.DeleteById(profile.Id);
                }
                await ms.Department.DeleteById(department.Id);
            }
            await ms.Company.DeleteById(company.Id);
            return Ok();
        }

        /*
        *  #TypeApp
        */
        [SwaggerOperation(Tags = new[] { "Rest/TypeApp" })]
        [HttpGet("GetTypeApps")]
        public async Task<IActionResult> GetTypeApps()
        {
            return Ok(await ms.TypeApp.GetAllDtos());
        }

        [SwaggerOperation(Tags = new[] { "Rest/TypeApp" })]
        [HttpPost("GetTypeApp")]
        public async Task<IActionResult> GetTypeApp([FromBody] IntIdModel? id)
        {
            //  todo return exception with required field
            if (id == null || id.Id == 0 || !ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            TypeAppDto? dto = await ms.TypeApp.GetDtoById(id.Id);
            if (dto == null)
                throw new NotFoundException("Запись не найдена");
            return Ok(dto);
        }

        [SwaggerOperation(Tags = new[] { "Rest/TypeApp" })]
        [HttpPost("AddTypeApp")]
        public async Task<IActionResult> AddTypeApp([FromBody] AddTypeAppModel model)
        {
            if (model == null || !ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            await ms.TypeApp.Save(model);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/TypeApp" })]
        [HttpPost("UpdateTypeApp")]
        public async Task<IActionResult> UpdateTypeApp([FromBody] UpdateTypeAppModel model)
        {
            if (model == null || model.Id <= 0 || !ModelState.IsValid)
                return BadRequest("Невалидное значение");
            TypeApp? TypeApp = await ms.TypeApp.GetById(model.Id);
            if (TypeApp == null)
                throw new NotFoundException("Запись не найдена");
            TypeApp? typeApp = await ms.TypeApp.GetById(model.Id);
            if (typeApp == null)
                throw new NotFoundException("Запись не найдена");
            await ms.TypeApp.Save(model);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/TypeApp" })]
        [HttpPost("DeleteTypeApp")]
        public async Task<IActionResult> DeleteTypeApp([FromBody] IntIdModel? id)
        {
            if (!ModelState.IsValid)//id == null || !id.Id.HasValue || id.Id == 0)
                return BadRequest("Невалидное значение");
            TypeApp? TypeApp = await ms.TypeApp.GetById(id.Id);
            if (TypeApp == null)
                throw new NotFoundException("Запись не найдена");
            await ms.TypeApp.DeleteById(TypeApp.Id);
            return Ok();
        }

        /*
        *  #App
        */
        [SwaggerOperation(Tags = new[] { "Rest/App" })]
        [HttpGet("GetApps")]
        public async Task<IActionResult> GetApps()
        {
            return Ok(await ms.App.GetAllDtos());
        }

        [SwaggerOperation(Tags = new[] { "Rest/App" })]
        [HttpPost("GetApp")]
        public async Task<IActionResult> GetApp([FromBody] IntIdModel? id)
        {
            if (id == null || id.Id == 0 || !ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            AppDto? dto = await ms.App.GetDtoById(id.Id);
            if (dto == null)
                throw new NotFoundException("Запись не найдена");
            return Ok(dto);
        }

        [SwaggerOperation(Tags = new[] { "Rest/App" })]
        [HttpPost("AddApp")]
        public async Task<IActionResult> AddApp([FromBody] AddAppModel model)
        {
            if (model == null || !ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            TypeApp? typeApp = await ms.TypeApp.GetById(model.IdTypeApp);
            if (typeApp == null)
                throw new NotFoundException("Запись не найдена");
            await ms.App.Save(model);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/App" })]
        [HttpPost("UpdateApp")]
        public async Task<IActionResult> UpdateApp([FromBody] UpdateAppModel model)
        {
            if (model == null || model.Id <= 0 || !ModelState.IsValid)
                return BadRequest("Невалидное значение");
            App? app = await ms.App.GetById(model.Id);
            TypeApp? typeApp = await ms.TypeApp.GetById(model.IdTypeApp);
            if (app == null || typeApp == null)
                throw new NotFoundException("Запись не найдена");
            await ms.App.Save(model);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/App" })]
        [HttpPost("DeleteApp")]
        public async Task<IActionResult> DeleteApp([FromBody] IntIdModel? id)
        {
            if (!ModelState.IsValid)//id == null || !id.Id.HasValue || id.Id == 0)
                return BadRequest("Невалидное значение");
            App? App = await ms.App.GetById(id.Id);
            if (App == null)
                throw new NotFoundException("Запись не найдена");
            await ms.App.DeleteById(App.Id);
            return Ok();
        }

        /*
        *  #Action
        */
        [SwaggerOperation(Tags = new[] { "Rest/Action" })]
        [HttpGet("GetActions")]
        public async Task<IActionResult> GetActions()
        {
            return Ok(await ms.Action.GetAllDtos());
        }

        [SwaggerOperation(Tags = new[] { "Rest/Action" })]
        [HttpPost("GetAction")]
        public async Task<IActionResult> GetAction([FromBody] IntIdModel? id)
        {
            //  todo return exception with required field
            if (id == null || id.Id == 0 || !ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            ActionDto? dto = await ms.Action.GetDtoById(id.Id);
            if (dto == null)
                throw new NotFoundException("Запись не найдена");
            return Ok(dto);
        }

        [SwaggerOperation(Tags = new[] { "Rest/Action" })]
        [HttpPost("GenerateActionForEmployee")]
        public async Task<IActionResult> GenerateActionForEmployee(int? IdEmployee, string dataRange)
        {
            if (!IdEmployee.HasValue || dataRange.IsNullOrEmpty() || !ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            Employee? employee = await ms.Employee.GetById(IdEmployee.Value);
            string[] dates = dataRange.Split("-");
            DateOnly startDate;
            DateOnly endDate;
            if (employee == null || 
                !DateOnly.TryParse(dates[0], out startDate) || !DateOnly.TryParse(dates[1], out endDate))
                throw new NotFoundException("Запись не найдена");


            Profile profile = await ms.Profile.GetById(employee.IdProfile.Value);
            if(profile==null)
                throw new NotFoundException("Запись не найдена");
            WorkTime workTime = await ms.WorkTime.GetByDepartmentId(profile.IdDepartment.Value);
            List<App> apps = await ms.App.GetAll();
            if (workTime != null && apps.Count != 0) {
                while (startDate <= endDate)
                {
                    if (!startDate.DayOfWeek.Equals(DayOfWeek.Saturday) && !startDate.DayOfWeek.Equals(DayOfWeek.Sunday))
                    {
                        Random rnd = new Random(DateTime.Now.Second);
                        TimeOnly startTime = workTime.StartTime.Value;
                        while (startTime <= workTime.EndTime)
                        {
                            int minutesOfApp = rnd.Next(10, 90);
                            TimeOnly endOfApp = startTime.AddMinutes(minutesOfApp);
                            App app = apps[rnd.Next(0, apps.Count)];
                            //actions.Add(new Action
                            await ms.Action.Save(new Action
                            {
                                IdEmployee = IdEmployee.Value,
                                IdApp = app.Id,
                                Date = startDate,
                                StartTime = startTime,
                                EndTime = endOfApp
                            });
                            startTime = startTime.AddMinutes(minutesOfApp);
                        }
                    }
                    startDate = startDate.AddDays(1);
                }
            }
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Action" })]
        [HttpPost("AddAction")]
        public async Task<IActionResult> AddAction([FromBody] AddActionModel model)
        {
            if (model == null || !ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            Employee? employee = await ms.Employee.GetById(model.IdEmployee);
            App? app = await ms.App.GetById(model.IdApp);
            if (employee == null || app == null)
                throw new NotFoundException("Запись не найдена");
            await ms.Action.Save(model);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Action" })]
        [HttpPost("UpdateAction")]
        public async Task<IActionResult> UpdateAction([FromBody] UpdateActionModel model)
        {
            if (model == null || model.Id <= 0 || !ModelState.IsValid)
                return BadRequest("Невалидное значение");
            Action? Action = await ms.Action.GetById(model.Id);
            if (Action == null)
                throw new NotFoundException("Запись не найдена");
            Employee? employee = await ms.Employee.GetById(model.IdEmployee);
            App? app = await ms.App.GetById(model.IdApp);
            if (employee == null || app == null)
                throw new NotFoundException("Запись не найдена");
            await ms.Action.Save(model);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Action" })]
        [HttpPost("DeleteAction")]
        public async Task<IActionResult> DeleteAction([FromBody] IntIdModel? id)
        {
            if (!ModelState.IsValid)//id == null || !id.Id.HasValue || id.Id == 0)
                return BadRequest("Невалидное значение");
            Action? Action = await ms.Action.GetById(id.Id);
            if (Action == null)
                throw new NotFoundException("Запись не найдена");
            await ms.Action.DeleteById(Action.Id);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Action" })]
        [HttpPost("DeleteAllActions")]
        public async Task<IActionResult> DeleteAllActions()
        {
            foreach (Action action in await ms.Action.GetAll())
            {
                await ms.Action.DeleteById(action.Id);
            }
            return Ok();
        }

        /*
         *  #WorkTime
         */
        [SwaggerOperation(Tags = new[] { "Rest/WorkTime" })]
        [HttpGet("GetWorkTimes")]
        public async Task<IActionResult> GetWorkTimes()
        {
            return Ok(await ms.WorkTime.GetAllDtos());
        }

        [SwaggerOperation(Tags = new[] { "Rest/WorkTime" })]
        [HttpPost("GetWorkTime")]
        public async Task<IActionResult> GetWorkTime([FromBody] IntIdModel? id)
        {
            //  todo return exception with required field
            if (id == null || id.Id == 0 || !ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            WorkTimeDto? dto = await ms.WorkTime.GetDtoById(id.Id);
            if (dto == null)
                throw new NotFoundException("Запись не найдена");
            return Ok(dto);
        }

        [SwaggerOperation(Tags = new[] { "Rest/WorkTime" })]
        [HttpPost("AddWorkTime")]
        public async Task<IActionResult> AddWorkTime([FromForm] AddWorkTimeModel model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest("Невалидное значение");
            logger.LogInformation($"/api/AddWorkTime POST idDepartment={model.IdDepartment} " +
                      $"startTime={model.StartTime} endTime={model.EndTime}");
            Department? department = await ms.Department.GetById(model.IdDepartment);
            if (department == null)
                throw new NotFoundException("Отдел не найдена");
            await ms.WorkTime.Save(model);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/WorkTime" })]
        [HttpPost("UpdateWorkTime")]
        public async Task<IActionResult> UpdateWorkTime([FromForm] UpdateWorkTimeModel? model)
        {
            if (model == null || model.Id <= 0 || !ModelState.IsValid)
                return BadRequest("Невалидное значение");
            logger.LogInformation($"/api/UpdateWorkTime POST id={model.Id} idDepartment={model.IdDepartment} " +
                                  $"startTime={model.StartTime} endTime={model.EndTime}");
            WorkTime? WorkTime = await ms.WorkTime.GetById(model.Id);
            if (WorkTime == null)
                return NotFound("Запись не найдена");
            Department? department = await ms.Department.GetById(model.IdDepartment);
            if (department == null)
                throw new NotFoundException("Отдел не найдена");
            await ms.WorkTime.Save(model);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/WorkTime" })]
        [HttpPost("DeleteWorkTime")]
        public async Task<IActionResult> DeleteWorkTime([FromBody] IntIdModel? id)
        {
            logger.LogInformation($"/api/DeleteWorkTime POST id={id.Id}");
            if (!ModelState.IsValid)//id == null || !id.Id.HasValue || id.Id == 0)
                throw new BadRequestException("Невалидное значение");
            WorkTime? WorkTime = await ms.WorkTime.GetById(id.Id);
            if (WorkTime == null)
                throw new NotFoundException("Запись не найдена");
            await ms.WorkTime.DeleteById(id.Id);
            return Ok();
        }

        /*
         *  #Department
         */
        [SwaggerOperation(Tags = new[] { "Rest/Department" })]
        [HttpGet("GetDepartments")]
        public async Task<IActionResult> GetDepartments() {
            return Ok(await ms.Department.GetAllDtos());
        }

        [SwaggerOperation(Tags = new[] { "Rest/Department" })]
        [HttpGet("GetDepartmentsByManagerHtml")]
        public async Task<IActionResult> GetDepartmentsByManagerHtml()
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
            string html = "";
            foreach(DepartmentDto dto in departmentDtos)
            {
                html += $"<option value=\"{dto.Id}\">{dto.Name}</option></br>";
            }
            return Content(html);
        }

        [SwaggerOperation(Tags = new[] { "Rest/Department" })]
        [HttpPost("GetDepartment")]
        public async Task<IActionResult> GetDepartment([FromBody] IntIdModel? id)
        {
            //  todo return exception with required field
            if (id == null  || id.Id == 0 || !ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            DepartmentDto? dto = await ms.Department.GetDtoById(id.Id);
            if (dto == null)
                throw new NotFoundException("Запись не найдена");
            return Ok(dto);
        }

        [SwaggerOperation(Tags = new[] { "Rest/Department" })]
        [HttpPost("AddDepartment")]
        public async Task<IActionResult> AddDepartment([FromBody] AddDepartmentModel? model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest("Невалидное значение");
            Company? company = await ms.Company.GetById(model.IdCompany);
            if (company == null)
                throw new NotFoundException("Компания не найдена");
            await ms.Department.Save(model);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Department" })]
        [HttpPost("UpdateDepartment")]
        public async Task<IActionResult> UpdateDepartment([FromBody] DepartmentDto? dto)
        {
            if(dto == null || dto.Id <= 0 || !ModelState.IsValid)
                return BadRequest("Невалидное значение");
            Department? department = await ms.Department.GetById(dto.Id);
            if (department == null)
                return NotFound("Запись не найдена");
            Company? company = await ms.Company.GetById(dto.IdCompany.Value);
            if (company == null)
                throw new NotFoundException("Компания не найдена");
            await ms.Department.Save(dto);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Department" })]
        [HttpPost("DeleteDepartment")]
        public async Task<IActionResult> DeleteDepartment([FromBody] IntIdModel id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Невалидное значение");
            List<Profile> profiles = (await ms.Profile.GetAll()).Where(x => x.IdDepartment != null && x.IdDepartment.Equals(id.Id)).ToList();
            foreach (Profile profile in profiles)
            {
                List<Employee> employees = (await ms.Employee.GetAll()).Where(x => x.IdProfile != null && x.IdProfile.Equals(id.Id)).ToList();
                List<Manager> managers = (await ms.Manager.GetAll()).Where(x => x.IdProfile != null && x.IdProfile.Equals(id.Id)).ToList();
                foreach (Employee employee in employees)
                {
                    await ms.Employee.DeleteById(employee.Id);
                }
                foreach (Manager manager in managers)
                {
                    await ms.Manager.DeleteById(manager.Id);
                }
                await ms.Profile.DeleteById(profile.Id);
            }
            await ms.Department.DeleteById(id.Id);
            return Ok();
        }

        /*
         *  #Profile
         */
        [SwaggerOperation(Tags = new[] { "Rest/Profile" })]
        [HttpGet("GetProfiles")]
        public async Task<IActionResult> GetProfiles()
        {
            return Ok(await ms.Profile.GetAllDtos());
        }

        [SwaggerOperation(Tags = new[] { "Rest/Profile" })]
        [HttpPost("GetProfilesByDepartmentHtml")]
        public async Task<IActionResult> GetProfilesByDepartmentHtml([FromBody] IntIdModel id)
        {
            if (id == null || id.Id == 0 || !ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            Department? department = await ms.Department.GetById(id.Id);
            if (department == null)
                throw new NotFoundException("Запись не найдена");

            var profileDtos = await ms.Profile.GetAllDtosByDepartment(id.Id);

            string html = "";
            foreach (ProfileDto dto in profileDtos)
            {
                html += $"<option value=\"{dto.Id}\">{dto.Name}</option></br>";
            }
            return Content(html);
        }

        [SwaggerOperation(Tags = new[] { "Rest/Profile" })]
        [HttpPost("GetProfile")]
        public async Task<IActionResult> GetProfile([FromBody] IntIdModel? id)
        {
            //  todo return exception with required field
            if (id == null || id.Id == 0 || !ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            ProfileDto? dto = await ms.Profile.GetDtoById(id.Id);
            if (dto == null)
                throw new NotFoundException("Запись не найдена");
            return Ok(dto);
        }

        [SwaggerOperation(Tags = new[] { "Rest/Profile" })]
        [HttpPost("AddProfile")]
        public async Task<IActionResult> AddProfile([FromBody] AddProfileModel? model)
        {
            if (model == null || !ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            Department? department = await ms.Department.GetById(model.IdDepartment);
            if (department == null)
                throw new BadRequestException("Отдел не найден");
            await ms.Profile.Save(model);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Profile" })]
        [HttpPost("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] ProfileDto? dto)
        {
            if (dto == null || dto.Id <= 0 || !ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            Profile? profile = await ms.Profile.GetById(dto.Id);
            if (profile == null)
                throw new NotFoundException("Запись не найдена");
            Department? department = await ms.Department.GetById(dto.IdDepartment.Value);
            if (department == null)
                throw new NotFoundException("Отдел не найдена");
            await ms.Profile.Save(dto);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Profile" })]
        [HttpPost("DeleteProfile")]
        public async Task<IActionResult> DeleteProfile([FromBody] IntIdModel? id)
        {
            if (!ModelState.IsValid)//id == null || !id.Id.HasValue || id.Id == 0)
                throw new BadRequestException("Невалидное значение");
            Profile? profile = await ms.Profile.GetById(id.Id);
            if (profile == null)
                throw new NotFoundException("Запись не найдена");
            await ms.Profile.DeleteById(id.Id);
            return Ok();
        }

        /*
        *  #Employee
        */
        [SwaggerOperation(Tags = new[] { "Rest/Employee" })]
        [HttpGet("GetEmployees")]
        public async Task<IActionResult> GetEmployees()
        {
            return Ok(await ms.Employee.GetAllDtos());
        }

        [SwaggerOperation(Tags = new[] { "Rest/Employee" })]
        [HttpPost("GetEmployee")]
        public async Task<IActionResult> GetEmployee([FromBody] IntIdModel? id)
        {
            //  todo return exception with required field
            if (id == null || id.Id == 0 || !ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            EmployeeDto? dto = await ms.Employee.GetDtoById(id.Id);
            if (dto == null)
                throw new NotFoundException("Запись не найдена");
            return Ok(dto);
        }
        //  todo password generate
        [SwaggerOperation(Tags = new[] { "Rest/Employee" })]
        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromBody] AddEmployeeModel? model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest("Невалидное значение");
            Profile? profile = await ms.Profile.GetById(model.IdProfile);
            if (profile == null)
                throw new NotFoundException("Профиль не найден");
            ClaimRole? claimRole = (await ms.ClaimRole.GetEmployeeRole());

            await ms.Employee.Save(model, claimRole);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Employee" })]
        [HttpPost("UpdateEmployee")]
        //todo notificate changing of password through email
        public async Task<IActionResult> UpdateEmployee([FromBody] UpdateEmployeeModel? model)
        {
            if (model == null || model.Id <= 0 || !ModelState.IsValid)
                return BadRequest("Невалидное значение");
            Employee? employee = await ms.Employee.GetById(model.Id);
            if (employee == null)
                throw new NotFoundException("Запись не найдена");
            Profile? profile = await ms.Profile.GetById(model.IdProfile);
            if (profile == null)
                throw new NotFoundException("Профиль не найден");
            ClaimRole? claimRole = (await ms.ClaimRole.GetEmployeeRole());

            await ms.Employee.Save(model, claimRole);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Employee" })]
        [HttpPost("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee([FromBody] IntIdModel? id)
        {
            if (id == null || !ModelState.IsValid)//id == null || !id.Id.HasValue || id.Id == 0)
                return BadRequest("Невалидное значение");
            Employee? company = await ms.Employee.GetById(id.Id);
            if (company == null)
                throw new NotFoundException("Запись не найдена");
            await ms.Employee.DeleteById(id.Id);
            return Ok();
        }

        /*
        *  #Manager
        */
        [SwaggerOperation(Tags = new[] { "Rest/Manager" })]
        [HttpGet("GetManagers")]
        public async Task<IActionResult> GetManagers()
        {
            return Ok(await ms.Manager.GetAllDtos());
        }

        [SwaggerOperation(Tags = new[] { "Rest/Manager" })]
        [HttpPost("GetManager")]
        public async Task<IActionResult> GetManager([FromBody] IntIdModel? id)
        {
            //  todo return exception with required field
            if (id == null || id.Id == 0 || !ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            logger.LogInformation($"api/Rest/Getmanager id={id.Id}");
            ManagerDto? dto = await ms.Manager.GetDtoById(id.Id);
            if (dto == null)
                throw new NotFoundException("Запись не найдена");
            return Ok(dto);
        }
        //  todo password generate
        [SwaggerOperation(Tags = new[] { "Rest/Manager" })]
        [HttpPost("AddManager")]
        public async Task<IActionResult> AddManager([FromBody] AddManagerModel? model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest("Невалидное значение");
            Profile? profile = await ms.Profile.GetById(model.IdProfile);
            if (profile == null)
                throw new NotFoundException("Профиль не найден");
            ClaimRole? claimRole = (await ms.ClaimRole.GetManagerRole());

            await ms.Manager.Save(model, claimRole);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Manager" })]
        [HttpPost("UpdateManager")]
        //todo notificate changing of password through email
        public async Task<IActionResult> UpdateManager([FromBody] UpdateManagerModel? model)
        {
            if (model == null || model.Id <= 0 || !ModelState.IsValid)
                return BadRequest("Невалидное значение");
            Manager? Manager = await ms.Manager.GetById(model.Id);
            if (Manager == null)
                throw new NotFoundException("Запись не найдена");
            Profile? profile = await ms.Profile.GetById(model.IdProfile.Value);
            if (profile == null)
                throw new NotFoundException("Профиль не найден");
            ClaimRole? claimRole = (await ms.ClaimRole.GetManagerRole());

            await ms.Manager.Save(model, claimRole);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Manager" })]
        [HttpPost("DeleteManager")]
        public async Task<IActionResult> DeleteManager([FromBody] IntIdModel? id)
        {
            if (id == null || !ModelState.IsValid)//id == null || !id.Id.HasValue || id.Id == 0)
                return BadRequest("Невалидное значение");
            Manager? company = await ms.Manager.GetById(id.Id);
            if (company == null)
                throw new NotFoundException("Запись не найдена");
            await ms.Manager.DeleteById(id.Id);
            return Ok();
        }
        /*
        *  #DepartmentManager
        */
        [SwaggerOperation(Tags = new[] { "Rest/DepartmentManager" })]
        [HttpGet("GetDepartmentManagers")]
        public async Task<IActionResult> GetDepartmentManagers()
        {
            return Ok(await ms.DepartmentManager.GetAllDtos());
        }

        [SwaggerOperation(Tags = new[] { "Rest/DepartmentManager" })]
        [HttpPost("GetDepartmentManager")]
        public async Task<IActionResult> GetDepartmentManager([FromBody] IntIdModel? id)
        {
            //  todo return exception with required field
            if (id == null || id.Id == 0 || !ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            DepartmentManagerDto? dto = await ms.DepartmentManager.GetDtoById(id.Id);
            if (dto == null)
                throw new NotFoundException("Запись не найдена");
            return Ok(dto);
        }
        //  todo password generate
        [SwaggerOperation(Tags = new[] { "Rest/DepartmentManager" })]
        [HttpPost("AddDepartmentManager")]
        public async Task<IActionResult> AddDepartmentManager([FromBody] AddDepartmentManagerModel? model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest("Невалидное значение");
            Manager? manager = await ms.Manager.GetById(model.IdManager);
            Department? department = await ms.Department.GetById(model.IdDepartment);
            if (manager == null || department == null)
                throw new NotFoundException("запись не найдена");

            await ms.DepartmentManager.Save(model);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/DepartmentManager" })]
        [HttpPost("UpdateDepartmentManager")]
        //todo notificate changing of password through email
        public async Task<IActionResult> UpdateDepartmentManager([FromBody] DepartmentManagerDto? dto)
        {
            if (dto == null || dto.Id <= 0 || !ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            DepartmentManager? DepartmentManager = await ms.DepartmentManager.GetById(dto.Id);
            if (DepartmentManager == null)
                throw new NotFoundException("Запись не найдена");
            Manager? manager = await ms.Manager.GetById(dto.IdManager);
            Department? department = await ms.Department.GetById(dto.IdDepartment);
            if (manager == null || department == null)
                throw new NotFoundException("Запись не найдена");
            await ms.DepartmentManager.Save(dto);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/DepartmentManager" })]
        [HttpPost("DeleteDepartmentManager")]
        public async Task<IActionResult> DeleteDepartmentManager([FromBody] IntIdModel? id)
        {
            if (id == null || !ModelState.IsValid)//id == null || !id.Id.HasValue || id.Id == 0)
                return BadRequest("Невалидное значение");
            DepartmentManager? company = await ms.DepartmentManager.GetById(id.Id);
            if (company == null)
                throw new NotFoundException("Запись не найдена");
            await ms.DepartmentManager.DeleteById(id.Id);
            return Ok();
        }

        /*
        *  #Admin
        */
        [SwaggerOperation(Tags = new[] { "Rest/Admin" })]
        [HttpGet("GetAdmins")]
        public async Task<IActionResult> GetAdmins()
        {
            return Ok(await ms.Admin.GetAllDtos());
        }

        [SwaggerOperation(Tags = new[] { "Rest/Admin" })]
        [HttpPost("GetAdmin")]
        public async Task<IActionResult> GetAdmin([FromBody] IntIdModel? id)
        {
            //  todo return exception with required field
            if (id == null || id.Id == 0 || !ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            AdminDto? dto = await ms.Admin.GetDtoById(id.Id);
            if (dto == null)
                throw new NotFoundException("Запись не найдена");
            return Ok(dto);
        }
        //  todo password generate
        [SwaggerOperation(Tags = new[] { "Rest/Admin" })]
        [HttpPost("AddAdmin")]
        public async Task<IActionResult> AddAdmin([FromBody] AddAdminModel? model)
        {
            if (model == null || !ModelState.IsValid)
                return BadRequest("Невалидное значение");
            ClaimRole? claimRole = (await ms.ClaimRole.GetAdminRole());

            await ms.Admin.Save(model, claimRole);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Admin" })]
        [HttpPost("UpdateAdmin")]
        //todo notificate changing of password through email
        public async Task<IActionResult> UpdateAdmin([FromBody] AdminDto? dto)
        {
            if (dto == null || dto.Id <= 0 || !ModelState.IsValid)
                return BadRequest("Невалидное значение");
            Admin? Admin = await ms.Admin.GetById(dto.Id);
            if (Admin == null)
                throw new NotFoundException("Запись не найдена");
            await ms.Admin.Save(dto);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Admin" })]
        [HttpPost("DeleteAdmin")]
        public async Task<IActionResult> DeleteAdmin([FromBody] IntIdModel? id)
        {
            if (id == null || !ModelState.IsValid)//id == null || !id.Id.HasValue || id.Id == 0)
                return BadRequest("Невалидное значение");
            Admin? company = await ms.Admin.GetById(id.Id);
            if (company == null)
                throw new NotFoundException("Запись не найдена");
            await ms.Admin.DeleteById(id.Id);
            return Ok();
        }
    }
}
