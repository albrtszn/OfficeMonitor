using AutoMapper;
using CRUD.implementation;
using DataBase.Repository.Models;
using OfficeMonitor.DTOs;
using OfficeMonitor.MiddleWares;
using OfficeMonitor.MiddleWares.Authorization;
using OfficeMonitor.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OfficeMonitor.Services
{
    public class EmployeeService
    {
        private EmployeeRepo EmployeeRepo;
        private ClaimRoleRepo ClaimRoleRepo;
        private IMapper mapper;
        private JwtProvider jwt;
        public EmployeeService(EmployeeRepo _EmployeeRepo, IMapper _mapper,
                               JwtProvider _jwt, ClaimRoleRepo claimRoleRepo)
        {
            EmployeeRepo = _EmployeeRepo;
            mapper = _mapper;
            jwt = _jwt;
            ClaimRoleRepo = claimRoleRepo;
        }

        public async Task<bool> DeleteById(int id)
        {
            return await EmployeeRepo.DeleteById(id);
        }

        public async Task<List<Employee>> GetAll()
        {
            return await EmployeeRepo.GetAll();
        }

        public async Task<List<EmployeeDto>> GetAllDtos()
        {
            List<EmployeeDto> Employees = new List<EmployeeDto>();
            List<Employee> list = await EmployeeRepo.GetAll();
            list.ForEach(x => Employees.Add(mapper.Map<EmployeeDto>(x)));
            return Employees;
        }

        public async Task<Employee?> GetById(int id)
        {
            return await EmployeeRepo.GetById(id);
        }

        public async Task<Employee?> GetByEmail(string email)
        {
            Employee? employee = (await EmployeeRepo.GetAll())
                .FirstOrDefault(x => x!= null && x.Login!= null 
                                && x.Login.Equals(email));
            return employee;
        }

        public async Task<string?> Login(string email, string password)
        {
            Employee? employee = await GetByEmail(email);
            if (employee == null)
                return null;
            if(PasswordHasher.Verify(password, employee.Password))
            {
                ClaimRole role = (await ClaimRoleRepo.GetById(employee.IdClaimRole.Value));
                string token = jwt.GenerateToken(employee, role!=null? role.Name : "USER");
                return token;
            }
            else
            {
                return null;
            }
        }

        public async Task<EmployeeDto> GetDtoById(int id)
        {
            return mapper.Map<EmployeeDto>(await EmployeeRepo.GetById(id));
        }

        public async Task<bool> Save(Employee EmployeeToSave)
        {
            EmployeeToSave.Password = PasswordHasher.Generate(EmployeeToSave.Password);
            return await EmployeeRepo.Save(EmployeeToSave);
        }

        public async Task<bool> Save(EmployeeDto EmployeeDtoToSave)
        {
            EmployeeDtoToSave.Password = PasswordHasher.Generate(EmployeeDtoToSave.Password);
            return await EmployeeRepo.Save(mapper.Map<Employee>(EmployeeDtoToSave));
        }

        public async Task<bool> Save(AddEmployeeModel EmployeeModelToSave)
        {
            EmployeeModelToSave.Password = PasswordHasher.Generate(EmployeeModelToSave.Password);
            return await EmployeeRepo.Save(mapper.Map<Employee>(EmployeeModelToSave));
        }        
        
        public async Task<bool> Save(AddEmployeeModel EmployeeModelToSave, ClaimRole? claimRole)
        {
            EmployeeModelToSave.Password = PasswordHasher.Generate(EmployeeModelToSave.Password);
            Employee employee = mapper.Map<Employee>(EmployeeModelToSave);
            if (claimRole != null)
                employee.IdClaimRole = claimRole.Id;
            return await EmployeeRepo.Save(employee);
        }
    }
}
