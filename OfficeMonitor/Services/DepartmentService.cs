using AutoMapper;
using CRUD.implementation;
using DataBase.Repository.Models;
using OfficeMonitor.DTOs;
using OfficeMonitor.Models;

namespace OfficeMonitor.Services
{
    public class DepartmentService
    {
        private DepartmentRepo DepartmentRepo;
        private IMapper mapper;
        public DepartmentService(DepartmentRepo _DepartmentRepo, IMapper _mapper)
        {
            DepartmentRepo = _DepartmentRepo;
            mapper = _mapper;
        }

        public async Task<bool> DeleteById(int id)
        {
            return await DepartmentRepo.DeleteById(id);
        }

        public async Task<List<Department>> GetAll()
        {
            return await DepartmentRepo.GetAll();
        }

        public async Task<List<DepartmentDto>> GetAllDtos()
        {
            List<DepartmentDto> Departments = new List<DepartmentDto>();
            List<Department> list = await DepartmentRepo.GetAll();
            list.ForEach(x => Departments.Add(mapper.Map<DepartmentDto>(x)));
            return Departments;
        }

        public async Task<Department?> GetById(int id)
        {
            return await DepartmentRepo.GetById(id);
        }

        public async Task<DepartmentDto> GetDtoById(int id)
        {
            return mapper.Map<DepartmentDto>(await DepartmentRepo.GetById(id));
        }

        public async Task<bool> Save(Department DepartmentToSave)
        {
            return await DepartmentRepo.Save(DepartmentToSave);
        }

        public async Task<bool> Save(DepartmentDto DepartmentDtoToSave)
        {
            return await DepartmentRepo.Save(mapper.Map<Department>(DepartmentDtoToSave));
        }

        public async Task<bool> Save(AddDepartmentModel DepartmentModelToSave)
        {
            return await DepartmentRepo.Save(mapper.Map<Department>(DepartmentModelToSave));
        }
    }
}
