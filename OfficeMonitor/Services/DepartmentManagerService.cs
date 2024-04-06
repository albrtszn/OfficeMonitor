using AutoMapper;
using CRUD.implementation;
using DataBase.Repository.Models;
using OfficeMonitor.DTOs;

namespace OfficeMonitor.Services
{
    public class DepartmentManagerService
    {
        private DepartmentManagerRepo DepartmentManagerRepo;
        private IMapper mapper;
        public DepartmentManagerService(DepartmentManagerRepo _DepartmentManagerRepo, IMapper _mapper)
        {
            DepartmentManagerRepo = _DepartmentManagerRepo;
            mapper = _mapper;
        }

        public async Task<bool> DeleteById(int id)
        {
            return await DepartmentManagerRepo.DeleteById(id);
        }

        public async Task<List<DepartmentManager>> GetAll()
        {
            return await DepartmentManagerRepo.GetAll();
        }

        public async Task<List<DepartmentManagerDto>> GetAllDtos()
        {
            List<DepartmentManagerDto> DepartmentManagers = new List<DepartmentManagerDto>();
            List<DepartmentManager> list = await DepartmentManagerRepo.GetAll();
            list.ForEach(x => DepartmentManagers.Add(mapper.Map<DepartmentManagerDto>(x)));
            return DepartmentManagers;
        }

        public async Task<DepartmentManager?> GetById(int id)
        {
            return await DepartmentManagerRepo.GetById(id);
        }

        public async Task<DepartmentManagerDto> GetDtoById(int id)
        {
            return mapper.Map<DepartmentManagerDto>(await DepartmentManagerRepo.GetById(id));
        }

        public async Task<bool> Save(DepartmentManager DepartmentManagerToSave)
        {
            return await DepartmentManagerRepo.Save(DepartmentManagerToSave);
        }

        public async Task<bool> Save(DepartmentManagerDto DepartmentManagerDtoToSave)
        {
            return await DepartmentManagerRepo.Save(mapper.Map<DepartmentManager>(DepartmentManagerDtoToSave));
        }
    }
}
