using AutoMapper;
using CRUD.implementation;
using DataBase.Repository.Models;
using OfficeMonitor.DTOs;
using OfficeMonitor.Models;

namespace OfficeMonitor.Services
{
    public class CompanyService
    {
        private CompanyRepo CompanyRepo;
        private IMapper mapper;
        public CompanyService(CompanyRepo _CompanyRepo, IMapper _mapper)
        {
            CompanyRepo = _CompanyRepo;
            mapper = _mapper;
        }

        public async Task<bool> DeleteById(int id)
        {
            return await CompanyRepo.DeleteById(id);
        }

        public async Task<List<Company>> GetAll()
        {
            return await CompanyRepo.GetAll();
        }

        public async Task<List<CompanyDto>> GetAllDtos()
        {
            List<CompanyDto> Companys = new List<CompanyDto>();
            List<Company> list = await CompanyRepo.GetAll();
            list.ForEach(x => Companys.Add(mapper.Map<CompanyDto>(x)));
            return Companys;
        }

        public async Task<Company?> GetById(int id)
        {
            return await CompanyRepo.GetById(id);
        }

        public async Task<CompanyDto> GetDtoById(int id)
        {
            return mapper.Map<CompanyDto>(await CompanyRepo.GetById(id));
        }

        public async Task<bool> Save(Company CompanyToSave)
        {
            return await CompanyRepo.Save(CompanyToSave);
        }

        public async Task<bool> Save(CompanyDto CompanyDtoToSave)
        {
            return await CompanyRepo.Save(mapper.Map<Company>(CompanyDtoToSave));
        }

        public async Task<bool> Save(AddCompanyModel CompanyModelToSave)
        {
            return await CompanyRepo.Save(mapper.Map<Company>(CompanyModelToSave));
        }
    }
}
