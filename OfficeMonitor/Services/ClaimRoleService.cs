using AutoMapper;
using CRUD.implementation;
using DataBase.Repository.Models;
using OfficeMonitor.DTOs;

namespace OfficeMonitor.Services
{
    public class ClaimRoleService
    {
        private ClaimRoleRepo ClaimRoleRepo;
        private IMapper mapper;
        public ClaimRoleService(ClaimRoleRepo _ClaimRoleRepo, IMapper _mapper)
        {
            ClaimRoleRepo = _ClaimRoleRepo;
            mapper = _mapper;
        }

        public async Task<bool> DeleteById(int id)
        {
            return await ClaimRoleRepo.DeleteById(id);
        }

        public async Task<List<ClaimRole>> GetAll()
        {
            return await ClaimRoleRepo.GetAll();
        }

        public async Task<List<ClaimRoleDto>> GetAllDtos()
        {
            List<ClaimRoleDto> ClaimRoles = new List<ClaimRoleDto>();
            List<ClaimRole> list = await ClaimRoleRepo.GetAll();
            list.ForEach(x => ClaimRoles.Add(mapper.Map<ClaimRoleDto>(x)));
            return ClaimRoles;
        }

        public async Task<ClaimRole?> GetById(int id)
        {
            return await ClaimRoleRepo.GetById(id);
        }

        public async Task<ClaimRoleDto> GetDtoById(int id)
        {
            return mapper.Map<ClaimRoleDto>(await ClaimRoleRepo.GetById(id));
        }

        public async Task<bool> Save(ClaimRole ClaimRoleToSave)
        {
            return await ClaimRoleRepo.Save(ClaimRoleToSave);
        }

        public async Task<bool> Save(ClaimRoleDto ClaimRoleDtoToSave)
        {
            return await ClaimRoleRepo.Save(mapper.Map<ClaimRole>(ClaimRoleDtoToSave));
        }
    }
}
