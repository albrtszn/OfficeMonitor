using AutoMapper;
using DataBase.Repository.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using OfficeMonitor.DTOs;
using OfficeMonitor.Models;
using OfficeMonitor.Services.MasterService;
using Swashbuckle.AspNetCore.Annotations;
using System.Text.Json.Serialization;

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
        public IActionResult Ping()
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

        /*
         *  Department
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
            if (id == null || !id.Id.HasValue || id.Id == 0)
                return BadRequest("ошибка:Невалидное значение");
            DepartmentDto? dto = await ms.Department.GetDtoById(id.Id.Value);
            if (dto == null)
                return NotFound("ошибка:Запись не найдена");
            return Ok(dto);
        }

        [SwaggerOperation(Tags = new[] { "Rest/Department" })]
        [HttpPost("AddDepartment")]
        public async Task<IActionResult> AddDepartment([FromBody] AddDepartmentModel? model)
        {
            if (model == null || model.Name.IsNullOrEmpty() || model.Description.IsNullOrEmpty())
                return BadRequest("ошибка:невалидное значение");
            await ms.Department.Save(model);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Department" })]
        [HttpPost("UpdateDepartment")]
        public async Task<IActionResult> UpdateDepartment([FromBody] DepartmentDto? dto)
        {
            if(dto == null || dto.Id <= 0 || dto.Name.IsNullOrEmpty() || dto.Description.IsNullOrEmpty())
                return BadRequest("ошибка:невалидное значение");
            Department? department = await ms.Department.GetById(dto.Id);
            if (department == null)
                return NotFound("ошибка:Запись не найдена");
            await ms.Department.Save(dto);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Department" })]
        [HttpDelete("DeleteDepartment")]
        public async Task<IActionResult> DeleteDepartment([FromBody] IntIdModel? id)
        {
            if (id == null || !id.Id.HasValue || id.Id == 0)
                return BadRequest("ошибка:невалидное значение");
            Department? department = await ms.Department.GetById(id.Id.Value);
            if (department == null)
                return NotFound("ошибка:Запись не найдена");
            await ms.Department.DeleteById(id.Id.Value);
            return Ok();
        }
    }
}
