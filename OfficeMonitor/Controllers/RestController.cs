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
using OfficeMonitor.Services;
using OfficeMonitor.Services.MasterService;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;
using Profile = DataBase.Repository.Models.Profile;

namespace OfficeMonitor.Controllers
{
    [Route("api/[controller]")]
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
        [HttpDelete("DeleteClaimRole")]
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
        [HttpDelete("DeletePlan")]
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
        [HttpDelete("DeleteCompany")]
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
         *  #Department
         */
        [SwaggerOperation(Tags = new[] { "Rest/Department" })]
        [HttpGet("GetDepartments")]
        public async Task<IActionResult> GetDepartments() {
            return Ok(await ms.Department.GetAllDtos());
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
        [HttpDelete("DeleteDepartment")]
        public async Task<IActionResult> DeleteDepartment([FromBody] IntIdModel? id)
        {
            if (!ModelState.IsValid)//id == null || !id.Id.HasValue || id.Id == 0)
                return BadRequest("Невалидное значение");
            Department? department = await ms.Department.GetById(id.Id);
            if (department == null)
                throw new NotFoundException("Запись не найдена");
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
        [HttpDelete("DeleteProfile")]
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
        [HttpDelete("DeleteEmployee")]
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
        [HttpDelete("DeleteManager")]
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
        [HttpDelete("DeleteDepartmentManager")]
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
        [HttpDelete("DeleteAdmin")]
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
