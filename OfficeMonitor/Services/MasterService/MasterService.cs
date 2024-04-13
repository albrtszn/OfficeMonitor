﻿namespace OfficeMonitor.Services.MasterService
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
    }
}
