using AutoMapper;
using DataBase.Repository.Models;
using OfficeMonitor.DTOs;
using Action = DataBase.Repository.Models.Action;
using Profile = DataBase.Repository.Models.Profile;

namespace OfficeMonitor.Mapper
{
    public class AppMappingProfile : AutoMapper.Profile
    {
        public AppMappingProfile() {
            CreateMap<Action, ActionDto>().ReverseMap();
            CreateMap<Admin, AdminDto>().ReverseMap();
            CreateMap<App, AppDto>().ReverseMap();
            CreateMap<CompanyDto, CompanyDto>().ReverseMap();
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
        }
    }
}
