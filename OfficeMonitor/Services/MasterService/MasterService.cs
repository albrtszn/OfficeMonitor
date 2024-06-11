using DataBase.Repository.Models;
using OfficeMonitor.DTOs;
using OfficeMonitor.Models.Employee;
using OfficeMonitor.Models.WorkTime;

namespace OfficeMonitor.Services.MasterService
{
    public class MasterService
    {
        public ActionService Action { get; }
        public AdminService Admin { get; }
        public AppService App { get; }
        public CompanyService Company{ get; }
        public CustomerRequestService CustomerRequest { get; }
        public DepartmentAppService DepartmentApp { get; }
        public DepartmentManagerService DepartmentManager { get; }
        public DepartmentService Department { get; }
        public EmployeeService Employee { get; }
        public ManagerService Manager { get; }
        public PlanService Plan { get; }
        public ProfileService Profile { get; }
        public TypeAppService TypeApp { get; }
        public WorkTimeService WorkTime { get; }
        public ClaimRoleService ClaimRole { get; }
        public TokenEmployeeService TokenEmployee { get; }
        public TokenManagerService TokenManager { get; }
        public TokenAdminService TokenAdmin { get; }
        public TokenCompanyService TokenCompany { get; }

        public MasterService(ActionService _Action, AdminService _Admin, AppService _App,
                             CompanyService _Company, CustomerRequestService _CustomerRequest,
                             DepartmentAppService _DepartmentApp, DepartmentManagerService _DepartmentManager,
                             DepartmentService _Department, EmployeeService _Employee,
                             ManagerService _Manager, PlanService _Plan, ProfileService _Profile,
                             TypeAppService _TypeApp, WorkTimeService _WorkTime, 
                             ClaimRoleService _ClaimRole, TokenEmployeeService _TokenEmployee,
                             TokenManagerService _TokenManager, TokenAdminService  _TokenAdmin,
                             TokenCompanyService _TokenCompany) { 
            Action = _Action;
            Admin = _Admin;
            App = _App;
            Company = _Company;
            CustomerRequest = _CustomerRequest;
            Department = _Department;
            DepartmentApp = _DepartmentApp;
            DepartmentManager = _DepartmentManager;
            Employee = _Employee;
            Manager = _Manager;
            Plan = _Plan;
            Profile = _Profile;
            TypeApp = _TypeApp;
            WorkTime = _WorkTime;
            ClaimRole = _ClaimRole;
            TokenEmployee = _TokenEmployee;
            TokenManager = _TokenManager;
            TokenManager = _TokenManager;
            TokenAdmin = _TokenAdmin;
            TokenCompany = _TokenCompany;
        }

        public async Task<bool> IsProfileExistsInDepartment(int profileId, int departmentId)
        {
            Profile? profile = await Profile.GetById(profileId);
            if(profile != null && profile.IdDepartment != null && profile.IdDepartment.Equals(departmentId)) {
                return true;
            }
            else
            {
                return false;
            }
        }
        public async Task<GetEmployeeModel> GetEmployeeModelbyId(int id)
        {
            Employee employee = await Employee.GetById(id);
            if(employee != null)
            {
                ProfileDto profile = await Profile.GetDtoById(employee.IdProfile.Value);
                if(profile != null)
                {
                    return new GetEmployeeModel
                    {
                        Id = employee.Id,
                        Name = employee.Name,
                        Surname = employee.Surname,
                        Patronamic = employee.Patronamic,
                        Login = employee.Login,
                        Password = employee.Password,
                        Department = await Department.GetDtoById(profile.IdDepartment.Value),
                        Profile = profile
                    };
                }
            }
            return null;
        }

        public async Task<List<Employee>> GetEmployeesByDepartment(int departmentId)
        {
            //var employees = (await Employee.GetAll()).Where(async x => x.IdProfile != null && (await IsProfileExistsInDepartment(x.IdProfile.Value, departmentId)));
            var employees = (await Employee.GetAll());
            List<Employee> employeesByDepartment = new List<Employee>();
            foreach(var employee in employees)
            {
                if(employee.IdProfile != null && await IsProfileExistsInDepartment(employee.IdProfile.Value, departmentId))
                    employeesByDepartment.Add(employee);
            }
            return employeesByDepartment;
        }
        public async Task<List<EmployeeDto>> GetEmployeeDtosByDepartment(int departmentId)
        {
            //var employees = (await Employee.GetAll()).Where(async x => x.IdProfile != null && (await IsProfileExistsInDepartment(x.IdProfile.Value, departmentId)));
            var employees = (await Employee.GetAllDtos());
            List<EmployeeDto> employeeDtosByDepartment = new List<EmployeeDto>();
            foreach (var employee in employees)
            {
                if (employee.IdProfile != null && await IsProfileExistsInDepartment(employee.IdProfile.Value, departmentId))
                    employeeDtosByDepartment.Add(employee);
            }
            return employeeDtosByDepartment;
        }

        public async Task<List<Manager>> GetManagersByDepartment(int departmentId)
        {
            //var employees = (await Employee.GetAll()).Where(async x => x.IdProfile != null && (await IsProfileExistsInDepartment(x.IdProfile.Value, departmentId)));
            var managers = (await Manager.GetAll());
            List<Manager> managersByDepartment = new List<Manager>();
            foreach (var manager in managers)
            {
                if (manager.IdProfile != null && await IsProfileExistsInDepartment(manager.IdProfile.Value, departmentId))
                    managersByDepartment.Add(manager);
            }
            return managersByDepartment;
        }
        public async Task<List<ManagerDto>> GetManagerDtosByDepartment(int departmentId)
        {
            //var employees = (await Employee.GetAll()).Where(async x => x.IdProfile != null && (await IsProfileExistsInDepartment(x.IdProfile.Value, departmentId)));
            var managers = (await Manager.GetAllDtos());
            List<ManagerDto> managerDtosByDepartment = new List<ManagerDto>();
            foreach (var manager in managers)
            {
                if (manager.IdProfile != null && await IsProfileExistsInDepartment(manager.IdProfile.Value, departmentId))
                    managerDtosByDepartment.Add(manager);
            }
            return managerDtosByDepartment;
        }

        public async Task<GetWorkTimeModel?> GetWorkTimeModel(int id)
        {
            GetWorkTimeModel? workTimeModel = null;
            WorkTime workTime = await WorkTime.GetById(id);
            if(workTime != null)
            {
                DepartmentDto departmentDto = await Department.GetDtoById(workTime.IdDepartment.Value);
                if (departmentDto != null)
                {
                    workTimeModel = new GetWorkTimeModel
                    {
                        Id = workTime.Id,
                        Department = departmentDto,
                        StartTime = workTime.StartTime.Value,
                        EndTime = workTime.EndTime.Value
                    };
                }
            }
            return workTimeModel;
        }
    }
}
