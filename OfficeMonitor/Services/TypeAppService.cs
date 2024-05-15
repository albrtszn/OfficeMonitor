using AutoMapper;
using CRUD.implementation;
using DataBase.Repository.Models;
using OfficeMonitor.DTOs;
using OfficeMonitor.Models.TypeApp;

namespace OfficeMonitor.Services
{
    public class TypeAppService
    {
        private TypeAppRepo TypeAppRepo;
        private IMapper mapper;
        public TypeAppService(TypeAppRepo _TypeAppRepo, IMapper _mapper)
        {
            TypeAppRepo = _TypeAppRepo;
            mapper = _mapper;
        }

        public async Task<bool> DeleteById(int id)
        {
            return await TypeAppRepo.DeleteById(id);
        }

        public async Task<List<TypeApp>> GetAll()
        {
            return await TypeAppRepo.GetAll();
        }

        public async Task<List<TypeAppDto>> GetAllDtos()
        {
            List<TypeAppDto> TypeApps = new List<TypeAppDto>();
            List<TypeApp> list = await TypeAppRepo.GetAll();
            list.ForEach(x => TypeApps.Add(mapper.Map<TypeAppDto>(x)));
            return TypeApps;
        }

        public async Task<TypeApp?> GetById(int id)
        {
            return await TypeAppRepo.GetById(id);
        }

        public async Task<TypeAppDto> GetDtoById(int id)
        {
            return mapper.Map<TypeAppDto>(await TypeAppRepo.GetById(id));
        }

        public async Task<bool> Save(TypeApp TypeAppToSave)
        {
            return await TypeAppRepo.Save(TypeAppToSave);
        }

        public async Task<bool> Save(TypeAppDto TypeAppDtoToSave)
        {
            return await TypeAppRepo.Save(mapper.Map<TypeApp>(TypeAppDtoToSave));
        }    
        
        public async Task<bool> Save(AddTypeAppModel TypeAppModelToSave)
        {
            return await TypeAppRepo.Save(mapper.Map<TypeApp>(TypeAppModelToSave));
        }      
        
        public async Task<bool> Save(UpdateTypeAppModel TypeAppModelToSave)
        {
            return await TypeAppRepo.Save(mapper.Map<TypeApp>(TypeAppModelToSave));
        }
    }
}
