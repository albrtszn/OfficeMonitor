﻿using AutoMapper;
using DataBase.Repository.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OfficeMonitor.DTOs;
using OfficeMonitor.ErrorHandler.Errors;
using OfficeMonitor.Models;
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
        /// <param name="item"></param>
        /// <returns>Status, token</returns>
        /// <remarks>
        /// Sample request:
        ///
        ///     POST /Todo
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
            string token = await ms.Employee.Login(model.Login, model.Password);
            if (token.Equals("error"))
                throw new NotFoundException("Запись не найдена");

            // todo cookie numberations
            HttpContext.Response.Cookies.Append("cookie#1", token);

            return Ok(new { message = $"log succesful. token:{token}" });
        }

        /*
        *  #Plan
        */
        [Authorize()]
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
                return BadRequest("Невалидное значение");
            Plan? plan = await ms.Plan.GetById(model.IdPlan);
            if (plan == null)
                throw new NotFoundException("План не найден");
            await ms.Company.Save(model);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Company" })]
        [HttpPost("UpdateCompany")]
        public async Task<IActionResult> UpdateCompany([FromBody] CompanyDto? dto)
        {
            if (dto == null || dto.Id <= 0 || !ModelState.IsValid)
                return BadRequest("Невалидное значение");
            Company? company = await ms.Company.GetById(dto.Id);
            if (company == null)
                throw new NotFoundException("Запись не найдена");
            Plan? plan = await ms.Plan.GetById(dto.IdPlan.Value);
            if (plan == null)
                throw new NotFoundException("План не найден");
            await ms.Company.Save(dto);
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
            await ms.Department.DeleteById(id.Id);
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
            await ms.Department.DeleteById(id.Id);
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
            await ms.Employee.Save(model);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Employee" })]
        [HttpPost("UpdateEmployee")]
        //todo notificate changing of password through email
        public async Task<IActionResult> UpdateEmployee([FromBody] EmployeeDto? dto)
        {
            if (dto == null || dto.Id <= 0 || !ModelState.IsValid)
                return BadRequest("Невалидное значение");
            Employee? employee = await ms.Employee.GetById(dto.Id);
            if (employee == null)
                throw new NotFoundException("Запись не найдена");
            Profile? profile = await ms.Profile.GetById(dto.IdProfile.Value);
            if (profile == null)
                throw new NotFoundException("Профиль не найден");
            await ms.Employee.Save(dto);
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
            await ms.Department.DeleteById(id.Id);
            return Ok();
        }
    }
}
