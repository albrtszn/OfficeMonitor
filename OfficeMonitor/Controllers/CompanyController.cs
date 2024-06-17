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
using OfficeMonitor.Models.Company;
using Action = DataBase.Repository.Models.Action;
using OfficeMonitor.Models.Profile;
using Swashbuckle.AspNetCore.Annotations;


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
            var planDto = await ms.Plan.GetDtoById(companyDto.IdPlan.Value);
            if (planDto == null)
                throw new NotFoundException($"План не найден. id={companyDto.IdPlan}");
            GetCompanyModel model = new GetCompanyModel
            {
                Login = companyDto.Login,
                Password = companyDto.Password,
                Name = companyDto.Name,
                Description = companyDto.Description,
                Plan = planDto,
                Balance = companyDto.Balance,
                IsActive = companyDto.IsActive.Value,
                IsBanned = companyDto.IsBanned.Value,
                DateOfEndPayment = companyDto.DateOfEndPayment,
                DateOfRegister = companyDto.DateOfRegister
            };
            return PartialView("PartialViews/CompanyInfo", model);
        }

        [Authorize(Roles = "COMPANY")]
        [HttpPost("GetEmployeesByDepartment")]
        public async Task<IActionResult> GetEmployeesByDepartment([FromBody] IntIdModel id)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int companyId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out companyId);
            var companyDto = await ms.Company.GetDtoById(companyId);
            if (companyDto == null)
                throw new NotFoundException($"Компания не найдена. id={companyId}");

            List<GetEmployeeOrManagerModel> workerModels = new List<GetEmployeeOrManagerModel>();
            foreach (Manager manager in await ms.GetManagersByDepartment(id.Id))
            {
                ProfileDto profileDto = null;
                DepartmentDto departmentDto = null;
                if (manager.IdProfile != null)
                {
                    profileDto = await ms.Profile.GetDtoById(manager.IdProfile.Value);
                    if (profileDto != null)
                        departmentDto = await ms.Department.GetDtoById(profileDto.IdDepartment.Value);
                }

                workerModels.Add(new GetEmployeeOrManagerModel
                {
                    Id = manager.Id,
                    IsManager = true,
                    Name = manager.Name,
                    Surname = manager.Surname,
                    Patronamic = manager.Patronamic,
                    Login = manager.Login,
                    Password = manager.Password,
                    Department = departmentDto,
                    Profile = profileDto
                });
            }
            foreach (Employee employe in await ms.GetEmployeesByDepartment(id.Id))
            {
                ProfileDto profileDto = null;
                DepartmentDto departmentDto = null;
                if (employe.IdProfile != null)
                {
                    profileDto = await ms.Profile.GetDtoById(employe.IdProfile.Value);
                    if (profileDto != null)
                        departmentDto = await ms.Department.GetDtoById(profileDto.IdDepartment.Value);
                }

                workerModels.Add(new GetEmployeeOrManagerModel
                {
                    Id = employe.Id,
                    IsManager = false,
                    Name = employe.Name,
                    Surname = employe.Surname,
                    Patronamic = employe.Patronamic,
                    Login = employe.Login,
                    Password = employe.Password,
                    Department = departmentDto,
                    Profile = profileDto
                });
            }
            return PartialView("Modal/DepartmentEmployees", workerModels);
        }

        [Authorize(Roles = "COMPANY")]
        [HttpGet("GetAddEmployee")]
        public async Task<IActionResult> GetAddEmployee()
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int companyId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out companyId);
            var companyDto = await ms.Company.GetById(companyId);
            if (companyDto == null)
                throw new NotFoundException($"Компания не найдена. id={companyId}");

            return PartialView("Modal/AddEmployeeContent");
        }
        [Authorize(Roles = "COMPANY")]
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

        [Authorize(Roles = "COMPANY")]
        [HttpPost("GetUpdateEmployee")]
        public async Task<IActionResult> GetUpdateEmployee([FromBody] IntIdModel id)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int companyId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out companyId);
            var companyDto = await ms.Company.GetById(companyId);
            if (companyDto == null)
                throw new NotFoundException($"Компания не найдена. id={companyId}");

            Employee employee = await ms.Employee.GetById(id.Id);
            if (employee == null)
                throw new NotFoundException("Сотрудник не найден");
            ProfileDto profile = await ms.Profile.GetDtoById(employee.IdProfile.Value);
            if (profile == null)
                throw new NotFoundException("Должность не найдена");
            DepartmentDto department = await ms.Department.GetDtoById(profile.IdDepartment.Value);

            GetEmployeeModel model = new GetEmployeeModel
            {
                Id = employee.Id,
                Name = employee.Name,
                Surname = employee.Surname,
                Patronamic = employee.Patronamic,
                Login = employee.Login,
                Password = employee.Password,
                Department = department,
                Profile = profile
            };
            return PartialView("Modal/UpdateEmployeeContent", model);
        }

        [Authorize(Roles = "COMPANY")]
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

            await ms.Employee.Save(model);
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Department" })]
        [HttpPost("DeleteEmployee")]
        public async Task<IActionResult> DeleteEmployee([FromBody] IntIdModel id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Невалидное значение");
            if (await ms.Employee.DeleteById(id.Id))
                throw new NotFoundException("Сотрудник не найден");

            await ms.Employee.DeleteById(id.Id);
            return Ok();
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
                    {
                        countOfManagers++;
                    }
                }

                departmentModels.Add(new GetDepartmentModel
                {
                    Id = department.Id,
                    Name = department.Name,
                    Description = department.Description,
                    IdCompany = companyId,
                    CountOfWorkers = countOfEmployees,
                    CountOfManagers = countOfManagers
                });
            }
            return PartialView("PartialViews/Departments", departmentModels);
        }
        
        [Authorize(Roles = "COMPANY")]
        [HttpGet("AddDepartment")]
        public async Task<IActionResult> AddDepartment()
        {
            return PartialView("Modal/AddDepartmentContent");
        }

        [Authorize(Roles = "COMPANY")]
        [HttpPost("AddDepartment")]
        public async Task<IActionResult> AddDepartment([FromBody] AddDepartmentCompanyModel model)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            logger.LogInformation($"Company/AddDepartment Post Name={model.Name} " +
                      $"Description={model.Description} count of profiles={model.Profiles.Count}");
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int companyId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out companyId);
            var companyDto = await ms.Company.GetById(companyId);
            if (companyDto == null)
                throw new NotFoundException($"Компания не найдена. id={companyId}");

            await ms.Department.Save(new Department
            {
                Name = model.Name,
                Description = model.Description,
                IdCompany = companyId
            });
            Department? department = (await ms.Department.GetAll()).FirstOrDefault(x => x.Name != null && x.Name.Equals(model.Name));
            if (department == null)
                throw new NotFoundException($"Отдел не найден. name={model.Name}");
            foreach(string profile in model.Profiles)
            {
                await ms.Profile.Save(new Profile
                {
                    Name = profile,
                    IdDepartment = department.Id
                });
            }
            return Ok();
        }

        [Authorize(Roles = "COMPANY")]
        [HttpPost("GetUpdateDepartment")]
        public async Task<IActionResult> GetUpdateDepartment([FromBody] IntIdModel id)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int companyId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out companyId);
            var companyDto = await ms.Company.GetById(companyId);
            if (companyDto == null)
                throw new NotFoundException($"Компания не найдена. id={companyId}");

            Department? department = await ms.Department.GetById(id.Id);
            if (department == null)
                throw new NotFoundException($"Отдел не найден. id={id.Id}");
            List<ProfileModel> profiles = new List<ProfileModel>();
            foreach(Profile profile in await ms.Profile.GetAllByDepartment(id.Id))
            {
                profiles.Add(new ProfileModel
                {
                    Id = profile.Id,
                    Name = profile.Name
                });
            }
            UpdateDepartmentCompanyModel model = new UpdateDepartmentCompanyModel
            {
                Id = department.Id,
                Name = department.Name,
                Description = department.Description,
                Profiles = profiles
            };
            return PartialView("Modal/UpdateDepartmentContent", model);
        }

        [Authorize(Roles = "COMPANY")]
        [HttpPost("UpdateDepartment")]
        public async Task<IActionResult> UpdateDepartment([FromBody] UpdateDepartmentCompanyModel model)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int companyId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out companyId);
            var companyDto = await ms.Company.GetById(companyId);
            if (companyDto == null)
                throw new NotFoundException($"Компания не найдена. id={companyId}");

            if (await ms.Department.GetById(model.Id) == null)
                throw new NotFoundException($"Отдел не найден. id={model.Id}");

            await ms.Department.Save(new Department
            {
                Id = model.Id,
                Name = model.Name,
                Description = model.Description,
                IdCompany = companyId
            });
            foreach (ProfileModel profile in model.Profiles)
            {
                if((await ms.Profile.GetAllByDepartment(model.Id)).FirstOrDefault(x=> x.Name!=null && x.Name.ToLower().Equals(profile.Name.ToLower()) ) == null)
                    await ms.Profile.Save(new Profile
                    {
                        Name = profile.Name,
                        IdDepartment = model.Id
                    });
            }
            logger.LogInformation($"Company/UpdateDepartment Post Id={model.Id} Name={model.Name} " +
                                  $"Description={model.Description} count of profiles={model.Profiles.Count}");
            return Ok();
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

        [Authorize(Roles = "COMPANY")]
        [HttpGet("GetDepartmentsByCompanyHtml")]
        public async Task<IActionResult> GetDepartmentsByCompanyHtml()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int companyId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out companyId);
            var companyDto = await ms.Company.GetDtoById(companyId);
            if (companyDto == null)
                throw new NotFoundException($"Компания не найдена. id={companyId}");

            var departments = (await ms.Department.GetAll()).Where(x => x != null && x.IdCompany != null && x.IdCompany.Equals(companyId));
            string html = "";
            foreach (Department dto in departments)
            {
                html += $"<option value=\"{dto.Id}\">{dto.Name}</option></br>";
            }
            return Content(html);
        }

        [Authorize(Roles = "COMPANY")]
        [HttpGet("GetDepartmentsByCompanyHtmlCheckBox")]
        public async Task<IActionResult> GetDepartmentsByCompanyHtmlCheckBox()
        {
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int companyId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out companyId);
            var companyDto = await ms.Company.GetDtoById(companyId);
            if (companyDto == null)
                throw new NotFoundException($"Компания не найдена. id={companyId}");

            var departments = (await ms.Department.GetAll()).Where(x => x != null && x.IdCompany != null && x.IdCompany.Equals(companyId));
            string html = "";
            int number = 1;
            foreach (Department dto in departments)
            {
                html += $"<input type=\"checkbox\" name=\"ManagedDepartments\" id=\"department{number}\" value=\"" + dto.Id + "\" />";
                html += $"<label for= \"department{number}\">" + dto.Name + "<label></br>";
                number++;
            }
            return Content(html);
        }
        [Authorize(Roles = "COMPANY")]
        [HttpPost("GetDepartmentsByCompanyHtmlCheckBoxByManager")]
        public async Task<IActionResult> GetDepartmentsByCompanyHtmlCheckBoxByManager([FromBody]IntIdModel id)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int companyId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out companyId);
            var companyDto = await ms.Company.GetDtoById(companyId);
            if (companyDto == null)
                throw new NotFoundException($"Компания не найдена. id={companyId}");

            var managedDepartments = (await ms.DepartmentManager.GetAllDtos()).Where(x=> x!=null && x.IdManager!=null &&
                                                                                         x.IdManager.Equals(id.Id));

            var departments = (await ms.Department.GetAll()).Where(x => x != null && x.IdCompany != null && x.IdCompany.Equals(companyId));
            string html = "";
            int number = 1;
            foreach (Department dto in departments)
            {
                if (managedDepartments.FirstOrDefault(x=>x.Id!=null && x.IdDepartment.Equals(dto.Id)) != null)
                {
                    html += $"<input type=\"checkbox\" name=\"ManagedDepartments\" id=\"department{number}\" value=\"" + dto.Id + "\" checked=\"checked\"/>";
                    html += $"<label for= \"department{number}\">" + dto.Name + "<label></br>";
                    number++;
                    }
                else {
                    html += $"<input type=\"checkbox\" name=\"ManagedDepartments\" id=\"department{number}\" value=\"" + dto.Id + "\" />";
                    html += $"<label for= \"department{number}\">" + dto.Name + "<label></br>";
                    number++;
                }
            }
            return Content(html);
        }

        [Authorize(Roles = "COMPANY")]
        [HttpGet("GetAddManager")]
        public async Task<IActionResult> GetAddManager()
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int companyId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out companyId);
            var companyDto = await ms.Company.GetById(companyId);
            if (companyDto == null)
                throw new NotFoundException($"Компания не найдена. id={companyId}");

            return PartialView("Modal/AddManagerContent");
        }
        [Authorize(Roles = "COMPANY")]
        [HttpPost("AddManager")]
        public async Task<IActionResult> AddManager([FromForm]AddManagerModel model)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int companyId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out companyId);
            var companyDto = await ms.Company.GetById(companyId);
            if (companyDto == null)
                throw new NotFoundException($"Компания не найдена. id={companyId}");
            ClaimRole? claimRole = (await ms.ClaimRole.GetManagerRole());

            await ms.Manager.Save(model, claimRole);

            Manager? manager = await ms.Manager.GetByEmail(model.Login);
            if(manager != null)
                foreach(int managerDepartmentId in model.ManagedDepartments)
                {
                    await ms.DepartmentManager.Save(new DepartmentManager
                    {
                        IdDepartment = managerDepartmentId,
                        IdManager = manager.Id
                    });
                }
            return Ok();
        }

        [Authorize(Roles = "COMPANY")]
        [HttpPost("GetUpdateManager")]
        public async Task<IActionResult> GetUpdateManager([FromBody] IntIdModel id)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int companyId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out companyId);
            var companyDto = await ms.Company.GetById(companyId);
            if (companyDto == null)
                throw new NotFoundException($"Компания не найдена. id={companyId}");

            Manager manager = await ms.Manager.GetById(id.Id);
            if (manager == null)
                throw new NotFoundException("Менеджер не найден");
            ProfileDto profile = await ms.Profile.GetDtoById(manager.IdProfile.Value);
            if (profile == null)
                throw new NotFoundException("Должность не найдена");
            DepartmentDto department = await ms.Department.GetDtoById(profile.IdDepartment.Value);

            GetManagerModel model = new GetManagerModel
            {
                Id = manager.Id,
                Name = manager.Name,
                Surname = manager.Surname,
                Patronamic = manager.Patronamic,
                Login = manager.Login,
                Password = manager.Password,
                Department = department,
                Profile = profile
            };
            return PartialView("Modal/UpdateManagerContent", model);
        }
        [Authorize(Roles = "COMPANY")]
        [HttpPost("UpdateManager")]
        public async Task<IActionResult> UpdateManager([FromForm] UpdateManagerModel model)
        {
            if (!ModelState.IsValid)
                throw new BadRequestException("Невалидное значение");
            var claimsIdentity = this.User.Identity as ClaimsIdentity;
            int companyId = 0;
            int.TryParse(claimsIdentity.FindFirst(x => x.Type.Contains("userId"))?.Value, out companyId);
            var companyDto = await ms.Company.GetById(companyId);
            if (companyDto == null)
                throw new NotFoundException($"Компания не найдена. id={companyId}");

            await ms.Manager.Save(model);

            Manager? manager = await ms.Manager.GetByEmail(model.Login);
            if (manager != null)
            {
                foreach(var managerDepartment in await ms.DepartmentManager.GetAllByManager(manager.Id))
                {
                    await ms.DepartmentManager.DeleteById(managerDepartment.Id);
                }
                foreach (int managerDepartmentId in model.ManagedDepartments)
                {
                    await ms.DepartmentManager.Save(new DepartmentManager
                    {
                        IdDepartment = managerDepartmentId,
                        IdManager = manager.Id
                    });
                }
            }
            return Ok();
        }

        [SwaggerOperation(Tags = new[] { "Rest/Department" })]
        [HttpPost("DeleteManager")]
        public async Task<IActionResult> DeleteManager([FromBody] IntIdModel id)
        {
            if (!ModelState.IsValid)
                return BadRequest("Невалидное значение");
            if (await ms.Employee.DeleteById(id.Id))
                throw new NotFoundException("Менеджер не найден");

            await ms.Manager.DeleteById(id.Id);
            return Ok();
        }
    }
}
