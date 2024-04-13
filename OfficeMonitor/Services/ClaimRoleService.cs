using AutoMapper;
using CRUD.implementation;
using DataBase.Repository.Models;
using Microsoft.IdentityModel.Tokens;
using OfficeMonitor.DTOs;
using OfficeMonitor.Models.ClaimRole;

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

        public async Task<ClaimRole?> GetEmployeeRole()
        {
            return (await ClaimRoleRepo.GetAll()).FirstOrDefault(x=> x!=null && !x.Name.IsNullOrEmpty()
                                                                     && x.Name.Equals("USER"));
        }
        public async Task<ClaimRole?> GetManagerRole()
        {
            return (await ClaimRoleRepo.GetAll()).FirstOrDefault(x => x != null && !x.Name.IsNullOrEmpty()
                                                                     && x.Name.Equals("MANAGER"));
        }
        public async Task<ClaimRole?> GetAdminRole()
        {
            return (await ClaimRoleRepo.GetAll()).FirstOrDefault(x => x != null && !x.Name.IsNullOrEmpty()
                                                                     && x.Name.Equals("ADMIN"));
        }
        public async Task<ClaimRole?> GetCompanyRole()
        {
            return (await ClaimRoleRepo.GetAll()).FirstOrDefault(x => x != null && !x.Name.IsNullOrEmpty()
                                                                     && x.Name.Equals("COMPANY"));
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

        public async Task<bool> Save(AddClaimRoleModel ClaimRoleDtoToSave)
        {
            return await ClaimRoleRepo.Save(mapper.Map<ClaimRole>(ClaimRoleDtoToSave));
        }        
        
        public async Task<bool> Save(UpdateClaimRoleModel ClaimRoleDtoToSave)
        {
            return await ClaimRoleRepo.Save(mapper.Map<ClaimRole>(ClaimRoleDtoToSave));
        }
    }
}
