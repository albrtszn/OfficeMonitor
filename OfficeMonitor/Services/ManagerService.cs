using AutoMapper;
using CRUD.implementation;
using OfficeMonitor.DataBase.Models;
using OfficeMonitor.DTOs;

namespace OfficeMonitor.Services
{
    public class ManagerService
    {
        private ManagerRepo ManagerRepo;
        private IMapper mapper;
        public ManagerService(ManagerRepo _ManagerRepo, IMapper _mapper)
        {
            ManagerRepo = _ManagerRepo;
            mapper = _mapper;
        }

        public async Task<bool> DeleteById(int id)
        {
            return await ManagerRepo.DeleteById(id);
        }

        public async Task<List<Manager>> GetAll()
        {
            return await ManagerRepo.GetAll();
        }

        public async Task<List<ManagerDto>> GetAllDtos()
        {
            List<ManagerDto> Managers = new List<ManagerDto>();
            List<Manager> list = await ManagerRepo.GetAll();
            list.ForEach(x => Managers.Add(mapper.Map<ManagerDto>(x)));
            return Managers;
        }

        public async Task<Manager?> GetById(int id)
        {
            return await ManagerRepo.GetById(id);
        }

        public async Task<ManagerDto> GetDtoById(int id)
        {
            return mapper.Map<ManagerDto>(await ManagerRepo.GetById(id));
        }

        public async Task<bool> Save(Manager ManagerToSave)
        {
            return await ManagerRepo.Save(ManagerToSave);
        }

        public async Task<bool> Save(ManagerDto ManagerDtoToSave)
        {
            return await ManagerRepo.Save(mapper.Map<Manager>(ManagerDtoToSave));
        }
    }
}
