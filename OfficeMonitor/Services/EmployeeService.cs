using AutoMapper;
using CRUD.implementation;
using DataBase.Repository.Models;
using OfficeMonitor.DTOs;

namespace OfficeMonitor.Services
{
    public class EmployeeService
    {
        private EmployeeRepo EmployeeRepo;
        private IMapper mapper;
        public EmployeeService(EmployeeRepo _EmployeeRepo, IMapper _mapper)
        {
            EmployeeRepo = _EmployeeRepo;
            mapper = _mapper;
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

        public async Task<EmployeeDto> GetDtoById(int id)
        {
            return mapper.Map<EmployeeDto>(await EmployeeRepo.GetById(id));
        }

        public async Task<bool> Save(Employee EmployeeToSave)
        {
            return await EmployeeRepo.Save(EmployeeToSave);
        }

        public async Task<bool> Save(EmployeeDto EmployeeDtoToSave)
        {
            return await EmployeeRepo.Save(mapper.Map<Employee>(EmployeeDtoToSave));
        }
    }
}
