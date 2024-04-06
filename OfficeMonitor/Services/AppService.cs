using AutoMapper;
using CRUD.implementation;
using DataBase.Repository.Models;
using OfficeMonitor.DTOs;

namespace OfficeMonitor.Services
{
    public class AppService
    {
        private AppRepo AppRepo;
        private IMapper mapper;
        public AppService(AppRepo _AppRepo, IMapper _mapper)
        {
            AppRepo = _AppRepo;
            mapper = _mapper;
        }

        public async Task<bool> DeleteById(int id)
        {
            return await AppRepo.DeleteById(id);
        }

        public async Task<List<App>> GetAll()
        {
            return await AppRepo.GetAll();
        }

        public async Task<List<AppDto>> GetAllDtos()
        {
            List<AppDto> Apps = new List<AppDto>();
            List<App> list = await AppRepo.GetAll();
            list.ForEach(x => Apps.Add(mapper.Map<AppDto>(x)));
            return Apps;
        }

        public async Task<App?> GetById(int id)
        {
            return await AppRepo.GetById(id);
        }

        public async Task<AppDto> GetDtoById(int id)
        {
            return mapper.Map<AppDto>(await AppRepo.GetById(id));
        }

        public async Task<bool> Save(App AppToSave)
        {
            return await AppRepo.Save(AppToSave);
        }

        public async Task<bool> Save(AppDto AppDtoToSave)
        {
            return await AppRepo.Save(mapper.Map<App>(AppDtoToSave));
        }
    }
}
