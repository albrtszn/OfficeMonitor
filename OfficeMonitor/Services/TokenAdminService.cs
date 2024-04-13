using AutoMapper;
using CRUD.implementation;
using DataBase.Repository.Models;
using OfficeMonitor.DTOs;

namespace OfficeMonitor.Services
{
    public class TokenAdminService
    {
        private TokenAdminRepo TokenAdminRepo;
        private IMapper mapper;
        public TokenAdminService(TokenAdminRepo _TokenAdminRepo, IMapper _mapper)
        {
            TokenAdminRepo = _TokenAdminRepo;
            mapper = _mapper;
        }

        public async Task<bool> DeleteById(int id)
        {
            return await TokenAdminRepo.DeleteById(id);
        }

        public async Task<List<TokenAdmin>> GetAll()
        {
            return await TokenAdminRepo.GetAll();
        }

        public async Task<List<TokenAdminDto>> GetAllDtos()
        {
            List<TokenAdminDto> TokenAdmins = new List<TokenAdminDto>();
            List<TokenAdmin> list = await TokenAdminRepo.GetAll();
            list.ForEach(x => TokenAdmins.Add(mapper.Map<TokenAdminDto>(x)));
            return TokenAdmins;
        }

        public async Task<TokenAdmin?> GetById(int id)
        {
            return await TokenAdminRepo.GetById(id);
        }

        public async Task<TokenAdminDto> GetDtoById(int id)
        {
            return mapper.Map<TokenAdminDto>(await TokenAdminRepo.GetById(id));
        }

        public async Task<bool> Save(TokenAdmin TokenAdminToSave)
        {
            return await TokenAdminRepo.Save(TokenAdminToSave);
        }

        public async Task<bool> Save(TokenAdminDto TokenAdminDtoToSave)
        {
            return await TokenAdminRepo.Save(mapper.Map<TokenAdmin>(TokenAdminDtoToSave));
        }
    }
}
