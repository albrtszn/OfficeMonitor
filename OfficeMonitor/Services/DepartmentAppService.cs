using AutoMapper;
using CRUD.implementation;
using OfficeMonitor.DataBase.Models;
using OfficeMonitor.DTOs;

namespace OfficeMonitor.Services
{
    public class DepartmentAppService
    {
        private DepartmentAppRepo DepartmentAppRepo;
        private IMapper mapper;
        public DepartmentAppService(DepartmentAppRepo _DepartmentAppRepo, IMapper _mapper)
        {
            DepartmentAppRepo = _DepartmentAppRepo;
            mapper = _mapper;
        }

        public async Task<bool> DeleteById(int id)
        {
            return await DepartmentAppRepo.DeleteById(id);
        }

        public async Task<List<DepartmentApp>> GetAll()
        {
            return await DepartmentAppRepo.GetAll();
        }

        public async Task<List<DepartmentAppDto>> GetAllDtos()
        {
            List<DepartmentAppDto> DepartmentApps = new List<DepartmentAppDto>();
            List<DepartmentApp> list = await DepartmentAppRepo.GetAll();
            list.ForEach(x => DepartmentApps.Add(mapper.Map<DepartmentAppDto>(x)));
            return DepartmentApps;
        }

        public async Task<DepartmentApp?> GetById(int id)
        {
            return await DepartmentAppRepo.GetById(id);
        }

        public async Task<DepartmentAppDto> GetDtoById(int id)
        {
            return mapper.Map<DepartmentAppDto>(await DepartmentAppRepo.GetById(id));
        }

        public async Task<bool> Save(DepartmentApp DepartmentAppToSave)
        {
            return await DepartmentAppRepo.Save(DepartmentAppToSave);
        }

        public async Task<bool> Save(DepartmentAppDto DepartmentAppDtoToSave)
        {
            return await DepartmentAppRepo.Save(mapper.Map<DepartmentApp>(DepartmentAppDtoToSave));
        }
    }
}
