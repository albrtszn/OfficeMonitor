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
using Swashbuckle.AspNetCore.Annotations;

namespace OfficeMonitor.Controllers
{
    [Route("Manager")]
    public class ManagerController : Controller
    {
        private readonly ILogger<ManagerController> logger;
        private MasterService ms;

        public ManagerController(ILogger<ManagerController> _logger, MasterService _ms)
        {
            logger = _logger;
            ms = _ms;
        }

        [Authorize(Roles = "MANAGER")]
        [HttpGet("GetDepartmentsByManagerHtml")]
        public async Task<IActionResult> GetDepartmentsByCompanyHtml()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int userId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out userId);
            var manager = await ms.Manager.GetById(userId);
            if (manager == null)
                throw new NotFoundException($"Компания не найдена. id={userId}");

            List<Department> departments = new List<Department>();
            foreach(var departmentManager in await ms.DepartmentManager.GetAllByManager(manager.Id))
            {
                Department? department = await ms.Department.GetById(departmentManager.IdDepartment.Value);
                if (department != null)
                    departments.Add(department);
            }

            string html = "";
            foreach (Department department in departments)
            {
                html += $"<option value=\"{department.Id}\">{department.Name}</option></br>";
            }
            return Content(html);
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

            var departmentsManager = (await ms.DepartmentManager.GetAll()).Where(x => x != null && x.IdManager != null && x.IdDepartment != null
                                                                                            && x.IdManager.Equals(managerId));

            var departments = (await ms.Department.GetAllDtos()).Where(x => x != null &&
                                                                            departmentsManager.Any(a => a.IdManager != null && a.IdManager.Equals(managerId) &&
                                                                                                      a.IdDepartment != null && a.IdDepartment.Equals(x.Id)));

            GetManagerModel managerModel = new GetManagerModel
            {
                Id = managerDto.Id,
                Name = managerDto.Name,
                Surname = managerDto.Surname,
                Patronamic = managerDto.Patronamic,
                Login = managerDto.Login,
                Password = managerDto.Password,
                Department = departmentDto,
                Profile = profileDto,
                ManagedDepartments = departments.ToList()
            };

            return PartialView("PartialViews/ManagerInfo", managerModel);
        }
        [Authorize(Roles = "MANAGER")]
        [HttpGet("GetDepartments")]
        public async Task<IActionResult> GetDepartments()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int managerId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out managerId);
            var managerDto = await ms.Manager.GetDtoById(managerId);
            if (managerDto == null)
                throw new NotFoundException($"Менеджер не найден. id={managerId}");

            var departmentsManager = (await ms.DepartmentManager.GetAll()).Where(x => x != null && x.IdManager != null && x.IdDepartment != null
                                                                                            && x.IdManager.Equals(managerId));

            var departments = (await ms.Department.GetAllDtos()).Where(x => x != null &&
                                                                            departmentsManager.Any(a => a.IdManager != null && a.IdManager.Equals(managerId) &&
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

                departmentModels.Add(new GetDepartmentModel
                {
                    Id = department.Id,
                    Name = department.Name,
                    Description = department.Description,
                    IdCompany = department.IdCompany.Value,
                    CountOfWorkers = countOfEmployees,
                    CountOfManagers = countOfManagers
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
            return PartialView("Modal/DepartmentEmployeesContent", models);
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

            return PartialView("Modal/AddEmployeeContent");
        }

        [Authorize(Roles = "MANAGER")]
        [HttpPost("AddEmployee")]
        public async Task<IActionResult> AddEmployee([FromForm] AddEmployeeModel model)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int companyId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out companyId);
            var companyDto = await ms.Company.GetById(companyId);
            if (companyDto == null)
                throw new NotFoundException($"Компания не найдена. id={companyId}");
            ClaimRole? claimRole = (await ms.ClaimRole.GetEmployeeRole());

            await ms.Employee.Save(model, claimRole);
            return Ok();
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
            if (model == null)
                throw new BadRequestException("Пользователь не найден");
            return PartialView("Modal/UpdateEmployeeContent", model);
        }

        [Authorize(Roles = "MANAGER")]
        [HttpPost("UpdateEmployee")]
        public async Task<IActionResult> UpdateEmployee([FromForm] UpdateEmployeeModel model)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int companyId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out companyId);
            var companyDto = await ms.Company.GetById(companyId);
            if (companyDto == null)
                throw new NotFoundException($"Компания не найдена. id={companyId}");
            ClaimRole? claimRole = (await ms.ClaimRole.GetEmployeeRole());

            await ms.Employee.Save(model, claimRole);
            return Ok();
        }

        [Authorize(Roles = "MANAGER")]
        [HttpPost("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee([FromBody] IntIdModel id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Невалидное значение");
            if (await ms.Employee.GetById(id.Id) == null)
                throw new NotFoundException("Сотрудник не найден");

            await ms.Employee.DeleteById(id.Id);
            return Ok();
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
                if (workTimeDto == null)
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
        public async Task<IActionResult> AddWorkTimeContent([FromBody] IntIdModel id)
        {
            if (!ModelState.IsValid)//id == null || !id.Id.HasValue || id.Id == 0)
                throw new BadRequestException("Невалидное значение");
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int managerId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out managerId);
            DepartmentDto departmentDto = await ms.Department.GetDtoById(id.Id);
            if (departmentDto == null)
                throw new NotFoundException($"Запись не найдена. id={id.Id}");

            return PartialView("Modal/AddWorkTimeContent", departmentDto);
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
            return PartialView("Modal/UpdateWorkTimeContent", model);
        }

        [Authorize(Roles = "MANAGER")]
        [HttpGet("DepartmentsStatistic")]
        public async Task<IActionResult> DepartmentsStatistic()
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
                                                                             departmentsManager.Any(a => a.IdDepartment != null && a.IdDepartment.Equals(model.DepartmentId)));

            DepartmentStatistic statistic = new DepartmentStatistic();
            List<EmployeeDto> employees = await ms.GetEmployeeDtosByDepartment(model.DepartmentId);
            WorkTimeDto? workTimeOfDepartment = await ms.WorkTime.GetDtoByDepartmentId(model.DepartmentId);

            double minutesPerDay = (workTimeOfDepartment.EndTime - workTimeOfDepartment.StartTime).Value.TotalMinutes;
            string[] dates = model.DateRange.Split(" - ");
            DateOnly startWorkDate;
            DateOnly endWorkDate;
            if (departmentDto != null && workTimeOfDepartment != null && !employees.IsNullOrEmpty() &&
                DateOnly.TryParse(dates[0], out startWorkDate) && DateOnly.TryParse(dates[1], out endWorkDate))
            {
                int totalMinutes = 0;
                int totalWorkedMinutes = 0;
                int totalIdleminutes = 0;
                int totalDiverMinutes = 0;
                foreach (var employee in employees)
                {
                    DateOnly currentDay = startWorkDate;
                    double employeeTotalMinutes = 0;
                    int employeeWorkedMinutes = 0;
                    int employeeIdleMinutes = 0;
                    int employeeDiverMinutes = 0;
                    int countOfDay = 0;
                    while (currentDay <= endWorkDate)
                    {
                        foreach (Action action in (await ms.Action.GetAllByEmployeeInDay(employee.Id, currentDay)).OrderBy(x => x.StartTime))
                        {
                            logger.LogInformation($"action id={action.Id} startTime={action.StartTime} endTime={action.EndTime}");
                            int actionTime = (int)(action.EndTime.Value.ToTimeSpan().TotalMinutes - action.StartTime.Value.ToTimeSpan().TotalMinutes);
                            App? app = await ms.App.GetById(action.IdApp.Value);
                            if (app != null)
                            {
                                if (app.IdTypeApp == 1)
                                {
                                    employeeWorkedMinutes += actionTime;
                                    totalWorkedMinutes += actionTime;
                                }
                                if (app.IdTypeApp == 2)
                                {
                                    employeeDiverMinutes += actionTime;
                                    totalDiverMinutes += actionTime;
                                }
                                employeeTotalMinutes += actionTime;
                                totalMinutes += actionTime;
                            }
                        }
                        currentDay = currentDay.AddDays(1);
                        countOfDay++;
                    }
                    statistic.Employees.Add(new GetEmployeeWithInfoModel
                    {
                        Id = employee.Id,
                        FIO = employee.Surname + " " + employee.Name + " " + employee.Patronamic,
                        Login = employee.Login,
                        Password = employee.Password,
                        Profile = await ms.Profile.GetDtoById(employee.IdProfile.Value),
                        WorkTime = new TimeSummaryModel
                        {
                            Hours = ((int)((employeeTotalMinutes - employeeIdleMinutes - employeeDiverMinutes) / 60)).ToString(),
                            Minutes = ((int)((employeeTotalMinutes - employeeIdleMinutes - employeeDiverMinutes) % 60)).ToString()
                        },
                        IdleTime = new TimeSummaryModel
                        {
                            Hours = ((int)((employeeTotalMinutes - employeeWorkedMinutes - employeeDiverMinutes) / 60)).ToString(),
                            Minutes = ((int)((employeeTotalMinutes - employeeWorkedMinutes - employeeDiverMinutes) % 60)).ToString()
                        },
                        DiversionTime = new TimeSummaryModel
                        {
                            Hours = ((int)((employeeTotalMinutes - employeeWorkedMinutes - employeeIdleMinutes) / 60)).ToString(),
                            Minutes = ((int)((employeeTotalMinutes - employeeWorkedMinutes - employeeIdleMinutes) % 60)).ToString()
                        }
                    });
                }

                double requiredMinutes = 0;
                while (startWorkDate <= endWorkDate)
                {
                    if (!startWorkDate.DayOfWeek.Equals(DayOfWeek.Saturday) && !startWorkDate.DayOfWeek.Equals(DayOfWeek.Sunday))
                        requiredMinutes += minutesPerDay;
                    startWorkDate = startWorkDate.AddDays(1);
                }

                statistic.Name = departmentDto.Name;
                statistic.WorkTime = workTimeOfDepartment;
                statistic.WorkedPercent = Math.Round((double)(totalMinutes - totalIdleminutes - totalDiverMinutes) / requiredMinutes * 100, 2);
                statistic.IdlePercent = Math.Round((double)(totalMinutes - totalWorkedMinutes - totalDiverMinutes) / totalMinutes * 100, 2);
                statistic.DiversionPercent = Math.Round((double)(totalMinutes - totalWorkedMinutes - totalIdleminutes) / totalMinutes * 100, 2);
                statistic.RequiredTotalHours = new TimeSummaryModel
                {
                    Hours = ((int)requiredMinutes / 60).ToString(),
                    Minutes = ((int)requiredMinutes % 60).ToString()
                };
                statistic.TotalHours = new TimeSummaryModel
                {
                    Hours = ((int)(totalMinutes / 60)).ToString(),
                    Minutes = ((int)(totalMinutes % 60)).ToString()
                };

                logger.LogInformation($"/GetDepartmentStatistic totalMinutes={totalMinutes} totalWorkedMinutes={totalWorkedMinutes}" +
                                      $"totalDiverMinutes={totalDiverMinutes} totalIdleMinutes={totalIdleminutes}");
            }
            return PartialView("PartialViews/GetDepartmentStatistic", statistic);
        }

        [Authorize(Roles = "MANAGER")]
        [HttpPost("GetEmployeeStatistic")]
        public async Task<IActionResult> GetEmployeeStatistic([FromBody] GetEmployeeStatistic model)
        {
            if (!ModelState.IsValid)//id == null || !id.Id.HasValue || id.Id == 0)
                throw new BadRequestException("Невалидное значение");
            logger.LogInformation($"/GetEmployeeStatistic POST employeeId={model.EmployeeId}, dataRane={model.DateRange}");

            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int managerId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out managerId);
            var managerDto = await ms.Manager.GetDtoById(managerId);
            if (managerDto == null)
                throw new NotFoundException($"Менеджер не найден. id={managerId}");


            EmployeeStatistic statistic = new EmployeeStatistic();
            EmployeeDto employeeDto = await ms.Employee.GetDtoById(model.EmployeeId);
            Profile profile = await ms.Profile.GetById(employeeDto.IdProfile.Value);
            WorkTimeDto? workTimeOfDepartment = await ms.WorkTime.GetDtoByDepartmentId(profile.IdDepartment.Value);
            statistic.Employee = employeeDto;
            statistic.WorkTime = workTimeOfDepartment;


            double minutesPerDay = (workTimeOfDepartment.EndTime - workTimeOfDepartment.StartTime).Value.TotalMinutes;
            string[] dates = model.DateRange.Split(" - ");
            DateOnly startWorkDate;
            DateOnly endWorkDate;
            if (workTimeOfDepartment != null && employeeDto != null &&
                DateOnly.TryParse(dates[0], out startWorkDate) && DateOnly.TryParse(dates[1], out endWorkDate))
            {
                int totalMinutes = 0;
                int totalWorkedMinutes = 0;
                int totalIdleminutes = 0;
                int totalDiverMinutes = 0;

                DateOnly currentDay = startWorkDate;

                double requiredMinutes = 0;
                while (startWorkDate <= endWorkDate)
                {
                    if (!startWorkDate.DayOfWeek.Equals(DayOfWeek.Saturday) && !startWorkDate.DayOfWeek.Equals(DayOfWeek.Sunday))
                        requiredMinutes += minutesPerDay;
                    startWorkDate = startWorkDate.AddDays(1);
                }

                int countOfDay = 0;
                while (currentDay <= endWorkDate)
                {
                    double employeeTotalMinutes = 0;
                    int employeeWorkedMinutes = 0;
                    int employeeIdleMinutes = 0;
                    int employeeDiverMinutes = 0;
                    foreach (Action action in (await ms.Action.GetAllByEmployeeInDay(employeeDto.Id, currentDay)).OrderBy(x => x.StartTime))
                    {
                        logger.LogInformation($"action id={action.Id} startTime={action.StartTime} endTime={action.EndTime}");
                        int actionTime = (int)(action.EndTime.Value.ToTimeSpan().TotalMinutes - action.StartTime.Value.ToTimeSpan().TotalMinutes);
                        App? app = await ms.App.GetById(action.IdApp.Value);
                        if (app != null)
                        {
                            if (app.IdTypeApp == 1)
                            {
                                employeeWorkedMinutes += actionTime;
                                totalWorkedMinutes += actionTime;
                            }
                            if (app.IdTypeApp == 2)
                            {
                                employeeDiverMinutes += actionTime;
                                totalDiverMinutes += actionTime;
                            }
                            employeeTotalMinutes += actionTime;
                            totalMinutes += actionTime;
                        }
                    }
                    statistic.Actions.Add(new DayStatistic
                    {
                        Date = currentDay,
                        WorkedPercent = Math.Round((double)(totalMinutes - totalIdleminutes - totalDiverMinutes) / requiredMinutes * 100, 2),
                        IdlePercent = Math.Round((double)(totalMinutes - totalWorkedMinutes - totalDiverMinutes) / totalMinutes * 100, 2),
                        DiversionPercent = Math.Round((double)(totalMinutes - totalWorkedMinutes - totalIdleminutes) / totalMinutes * 100, 2)
                });
                currentDay = currentDay.AddDays(1);
                    countOfDay++;
                }

                statistic.RequiredTotalHours = new TimeSummaryModel
                {
                    Hours = ((int)requiredMinutes / 60).ToString(),
                    Minutes = ((int)requiredMinutes % 60).ToString()
                };
                statistic.TotalHours = new TimeSummaryModel
                {
                    Hours = ((int)(totalMinutes / 60)).ToString(),
                    Minutes = ((int)(totalMinutes % 60)).ToString()
                };

                logger.LogInformation($"/GetDepartmentStatistic totalMinutes={totalMinutes} totalWorkedMinutes={totalWorkedMinutes}" +
                                      $"totalDiverMinutes={totalDiverMinutes} totalIdleMinutes={totalIdleminutes}");
            }
            return PartialView("PartialViews/GetEmployeeStatistic", statistic);
        }
    }
}
