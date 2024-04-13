using AutoMapper;
using CRUD.implementation;
using DataBase.Repository.Models;
using OfficeMonitor.DTOs;

namespace OfficeMonitor.Services
{
    public class TokenCompanyService
    {
        private TokenCompanyRepo TokenCompanyRepo;
        private IMapper mapper;
        public TokenCompanyService(TokenCompanyRepo _TokenCompanyRepo, IMapper _mapper)
        {
            TokenCompanyRepo = _TokenCompanyRepo;
            mapper = _mapper;
        }

        public async Task<bool> DeleteById(int id)
        {
            return await TokenCompanyRepo.DeleteById(id);
        }

        public async Task<List<TokenCompany>> GetAll()
        {
            return await TokenCompanyRepo.GetAll();
        }

        public async Task<List<TokenCompanyDto>> GetAllDtos()
        {
            List<TokenCompanyDto> TokenCompanys = new List<TokenCompanyDto>();
            List<TokenCompany> list = await TokenCompanyRepo.GetAll();
            list.ForEach(x => TokenCompanys.Add(mapper.Map<TokenCompanyDto>(x)));
            return TokenCompanys;
        }

        public async Task<TokenCompany?> GetById(int id)
        {
            return await TokenCompanyRepo.GetById(id);
        }

        public async Task<TokenCompanyDto> GetDtoById(int id)
        {
            return mapper.Map<TokenCompanyDto>(await TokenCompanyRepo.GetById(id));
        }

        public async Task<bool> Save(TokenCompany TokenCompanyToSave)
        {
            return await TokenCompanyRepo.Save(TokenCompanyToSave);
        }

        public async Task<bool> Save(TokenCompanyDto TokenCompanyDtoToSave)
        {
            return await TokenCompanyRepo.Save(mapper.Map<TokenCompany>(TokenCompanyDtoToSave));
        }
    }
}
