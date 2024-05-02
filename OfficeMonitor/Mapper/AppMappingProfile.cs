using AutoMapper;
using DataBase.Repository.Models;
using OfficeMonitor.DTOs;
using OfficeMonitor.Models;
using OfficeMonitor.Models.ClaimRole;
using OfficeMonitor.Models.Company;
using OfficeMonitor.Models.Departments;
using OfficeMonitor.Models.Employee;
using OfficeMonitor.Models.Manager;
using OfficeMonitor.Models.WorkTime;
using Action = DataBase.Repository.Models.Action;
using Profile = DataBase.Repository.Models.Profile;

namespace OfficeMonitor.Mapper
{
    public class AppMappingProfile : AutoMapper.Profile
    {
        public AppMappingProfile() {
            /*
             *  DTOs
             */
            CreateMap<Action, ActionDto>().ReverseMap();
            CreateMap<Admin, AdminDto>().ReverseMap();
            CreateMap<App, AppDto>().ReverseMap();
            CreateMap<Company, CompanyDto>().ReverseMap();
            CreateMap<CustomerRequest, CustomerRequestDto>().ReverseMap();
            CreateMap<Department, DepartmentDto>().ReverseMap();
            CreateMap<DepartmentApp, DepartmentAppDto>().ReverseMap();
            CreateMap<DepartmentManager, DepartmentManagerDto>().ReverseMap();
            CreateMap<Employee, EmployeeDto>().ReverseMap();
            CreateMap<Manager, ManagerDto>().ReverseMap();
            CreateMap<Plan, PlanDto>().ReverseMap();
            CreateMap<Profile, ProfileDto>().ReverseMap();
            CreateMap<TypeApp, TypeAppDto>().ReverseMap();
            CreateMap<WorkTime, WorkTimeDto>().ReverseMap();
            CreateMap<ClaimRole, ClaimRoleDto>().ReverseMap();
            CreateMap<TokenEmployee, TokenEmployeeDto>().ReverseMap();
            CreateMap<TokenManager, TokenManagerDto>().ReverseMap();
            CreateMap<TokenAdmin, TokenAdminDto>().ReverseMap();
            CreateMap<TokenCompany, TokenCompanyDto>().ReverseMap();

            /*
             *  Models
             */
            // todo models mapping
            //  AddModels
            CreateMap<Plan, AddPlanModel>().ReverseMap();
            CreateMap<Company, AddCompanyModel>().ReverseMap();
            CreateMap<Department, AddDepartmentModel>().ReverseMap();
            CreateMap<WorkTime, AddWorkTimeModel>().ReverseMap();
            CreateMap<DepartmentManager, AddDepartmentManagerModel>().ReverseMap();
            CreateMap<Profile, AddProfileModel>().ReverseMap();
            CreateMap<Employee, AddEmployeeModel>().ReverseMap();
            CreateMap<Manager, AddManagerModel>().ReverseMap();
            CreateMap<Admin, AddAdminModel>().ReverseMap();
            CreateMap<ClaimRole, AddClaimRoleModel>().ReverseMap();
            //  UpdateModels
            CreateMap<ClaimRole, UpdateClaimRoleModel>().ReverseMap();
            CreateMap<Employee, UpdateEmployeeModel>().ReverseMap();
            CreateMap<Manager, UpdateManagerModel>().ReverseMap();
            //CreateMap<Admin, UpdateAdminModel>().ReverseMap();
            CreateMap<Company, UpdateCompanyModel>().ReverseMap();
            CreateMap<WorkTime, UpdateWorkTimeModel>().ReverseMap();
            
        }
    }
}
