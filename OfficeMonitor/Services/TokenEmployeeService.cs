using AutoMapper;
using CRUD.implementation;
using DataBase.Repository.Models;
using OfficeMonitor.DTOs;

namespace OfficeMonitor.Services
{
    public class TokenEmployeeService
    {
        private TokenEmployeeRepo TokenEmployeeRepo;
        private IMapper mapper;
        public TokenEmployeeService(TokenEmployeeRepo _TokenEmployeeRepo, IMapper _mapper)
        {
            TokenEmployeeRepo = _TokenEmployeeRepo;
            mapper = _mapper;
        }

        public async Task<bool> DeleteById(int id)
        {
            return await TokenEmployeeRepo.DeleteById(id);
        }

        public async Task<List<TokenEmployee>> GetAll()
        {
            return await TokenEmployeeRepo.GetAll();
        }

        public async Task<List<TokenEmployeeDto>> GetAllDtos()
        {
            List<TokenEmployeeDto> TokenEmployees = new List<TokenEmployeeDto>();
            List<TokenEmployee> list = await TokenEmployeeRepo.GetAll();
            list.ForEach(x => TokenEmployees.Add(mapper.Map<TokenEmployeeDto>(x)));
            return TokenEmployees;
        }

        public async Task<TokenEmployee?> GetById(int id)
        {
            return await TokenEmployeeRepo.GetById(id);
        }

        public async Task<TokenEmployeeDto> GetDtoById(int id)
        {
            return mapper.Map<TokenEmployeeDto>(await TokenEmployeeRepo.GetById(id));
        }

        public async Task<bool> Save(TokenEmployee TokenEmployeeToSave)
        {
            return await TokenEmployeeRepo.Save(TokenEmployeeToSave);
        }

        public async Task<bool> Save(TokenEmployeeDto TokenEmployeeDtoToSave)
        {
            return await TokenEmployeeRepo.Save(mapper.Map<TokenEmployee>(TokenEmployeeDtoToSave));
        }
    }
}
