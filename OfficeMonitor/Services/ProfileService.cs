using AutoMapper;
using CRUD.implementation;
using OfficeMonitor.DTOs;
using Profile = OfficeMonitor.DataBase.Models.Profile;

namespace OfficeMonitor.Services
{
    public class ProfileService
    {
        private ProfileRepo ProfileRepo;
        private IMapper mapper;
        public ProfileService(ProfileRepo _ProfileRepo, IMapper _mapper)
        {
            ProfileRepo = _ProfileRepo;
            mapper = _mapper;
        }

        public async Task<bool> DeleteById(int id)
        {
            return await ProfileRepo.DeleteById(id);
        }

        public async Task<List<Profile>> GetAll()
        {
            return await ProfileRepo.GetAll();
        }

        public async Task<List<ProfileDto>> GetAllDtos()
        {
            List<ProfileDto> Profiles = new List<ProfileDto>();
            List<Profile> list = await ProfileRepo.GetAll();
            list.ForEach(x => Profiles.Add(mapper.Map<ProfileDto>(x)));
            return Profiles;
        }

        public async Task<Profile?> GetById(int id)
        {
            return await ProfileRepo.GetById(id);
        }

        public async Task<ProfileDto> GetDtoById(int id)
        {
            return mapper.Map<ProfileDto>(await ProfileRepo.GetById(id));
        }

        public async Task<bool> Save(Profile ProfileToSave)
        {
            return await ProfileRepo.Save(ProfileToSave);
        }

        public async Task<bool> Save(ProfileDto ProfileDtoToSave)
        {
            return await ProfileRepo.Save(mapper.Map<Profile>(ProfileDtoToSave));
        }
    }
}
