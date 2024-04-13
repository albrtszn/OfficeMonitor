using AutoMapper;
using CRUD.implementation;
using DataBase.Repository.Models;
using OfficeMonitor.DTOs;

namespace OfficeMonitor.Services
{
    public class TokenManagerService
    {
        private TokenManagerRepo TokenManagerRepo;
        private IMapper mapper;
        public TokenManagerService(TokenManagerRepo _TokenManagerRepo, IMapper _mapper)
        {
            TokenManagerRepo = _TokenManagerRepo;
            mapper = _mapper;
        }

        public async Task<bool> DeleteById(int id)
        {
            return await TokenManagerRepo.DeleteById(id);
        }

        public async Task<List<TokenManager>> GetAll()
        {
            return await TokenManagerRepo.GetAll();
        }

        public async Task<List<TokenManagerDto>> GetAllDtos()
        {
            List<TokenManagerDto> TokenManagers = new List<TokenManagerDto>();
            List<TokenManager> list = await TokenManagerRepo.GetAll();
            list.ForEach(x => TokenManagers.Add(mapper.Map<TokenManagerDto>(x)));
            return TokenManagers;
        }

        public async Task<TokenManager?> GetById(int id)
        {
            return await TokenManagerRepo.GetById(id);
        }

        public async Task<TokenManagerDto> GetDtoById(int id)
        {
            return mapper.Map<TokenManagerDto>(await TokenManagerRepo.GetById(id));
        }

        public async Task<bool> Save(TokenManager TokenManagerToSave)
        {
            return await TokenManagerRepo.Save(TokenManagerToSave);
        }

        public async Task<bool> Save(TokenManagerDto TokenManagerDtoToSave)
        {
            return await TokenManagerRepo.Save(mapper.Map<TokenManager>(TokenManagerDtoToSave));
        }
    }
}
