using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using OfficeMonitor.DataBase.Models;
using OfficeMonitor.DTOs;
using System.Text.Json.Serialization;

namespace OfficeMonitor.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestController : ControllerBase
    {
        private readonly ILogger<HomeController> logger;
        private IMapper mapper;

        public RestController(ILogger<HomeController> _logger, IMapper _mapper)
        {
            logger = _logger;
            mapper = _mapper;
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

            return Ok(new { message = $"{DateTime.Now} Employee type: {emplDto.GetType()} ." , emplDto});
        }
    }
}
