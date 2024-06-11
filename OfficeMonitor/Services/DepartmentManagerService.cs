using AutoMapper;
using CRUD.implementation;
using DataBase.Repository.Models;
using OfficeMonitor.DTOs;
using OfficeMonitor.Models;

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

        public async Task<List<DepartmentManager>> GetAllByManager(int managerId)
        {
            return (await DepartmentManagerRepo.GetAll()).Where(x => x.IdManager != null && x.IdManager.Equals(managerId)).ToList();
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

        public async Task<bool> Save(AddDepartmentManagerModel DepartmentManagerModelToSave)
        {
            return await DepartmentManagerRepo.Save(mapper.Map<DepartmentManager>(DepartmentManagerModelToSave));
        }
    }
}
