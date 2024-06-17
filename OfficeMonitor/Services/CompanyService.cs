using AutoMapper;
using CRUD.implementation;
using DataBase.Repository.Models;
using OfficeMonitor.DTOs;
using OfficeMonitor.MiddleWares;
using OfficeMonitor.MiddleWares.Authorization;
using OfficeMonitor.Models.Company;

namespace OfficeMonitor.Services
{
    public class CompanyService
    {
        private CompanyRepo CompanyRepo;
        private ClaimRoleRepo ClaimRoleRepo;
        private TokenCompanyRepo TokenCompanyRepo;
        private IMapper mapper;
        private JwtProvider jwt;
        public CompanyService(CompanyRepo _CompanyRepo, IMapper _mapper,
                              JwtProvider _jwt, ClaimRoleRepo _ClaimRoleRepo, 
                              TokenCompanyRepo _TokenCompanyRepo)
        {
            CompanyRepo = _CompanyRepo;
            mapper = _mapper;
            jwt = _jwt;
            ClaimRoleRepo = _ClaimRoleRepo;
            TokenCompanyRepo = _TokenCompanyRepo;
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

        public async Task<Company?> GetByEmail(string email)
        {
            Company? company = (await CompanyRepo.GetAll())
                .FirstOrDefault(x => x != null && x.Login != null
                                && x.Login.Equals(email));
            return company;
        }

        public async Task<string?> Login(string email, string password)
        {
            Company? company = await GetByEmail(email);
            if (company == null)
                return null;
            if (PasswordHasher.Verify(password, company.Password))
            {
                TokenCompany? tokenCompany = await TokenCompanyRepo.GetByCompanyId(company.Id);
                if (tokenCompany != null && !TokenCompanyRepo.IsTokenExpired(tokenCompany))
                {
                    return tokenCompany.Token;
                }
                else
                {
                    if(tokenCompany != null)
                        await TokenCompanyRepo.DeleteById(tokenCompany.Id);
                    ClaimRole? role = (await ClaimRoleRepo.GetById(company.IdClaimRole.Value));
                    string token = jwt.GenerateToken(company, role != null ? role.Name : "COMPANY");
                    await TokenCompanyRepo.Save(new TokenCompany
                    {
                        IdCompany = company.Id,
                        Token = token,
                        DateOfCreation = DateTime.Now
                    });
                    return token;
                }
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> Save(Company CompanyToSave)
        {
            CompanyToSave.Password = PasswordHasher.Generate(CompanyToSave.Password);
            return await CompanyRepo.Save(CompanyToSave);
        }

        public async Task<bool> Save(CompanyDto CompanyDtoToSave)
        {
            CompanyDtoToSave.Password = PasswordHasher.Generate(CompanyDtoToSave.Password);
            return await CompanyRepo.Save(mapper.Map<Company>(CompanyDtoToSave));
        }        

        public async Task<bool> Save(AddCompanyModel CompanyModelToSave)
        {
            Company company = mapper.Map<Company>(CompanyModelToSave);
            company.Password = PasswordHasher.Generate(CompanyModelToSave.Password);
            company.Balance = 0;
            company.DateOfRegister = DateTime.Now;
            return await CompanyRepo.Save(company);
        }

        public async Task<bool> Save(AddCompanyModel CompanyModelToSave, ClaimRole? claimRole)
        {
            Company company = mapper.Map<Company>(CompanyModelToSave);
            company.Balance = 0;
            company.DateOfRegister = DateTime.Now;
            company.Password = PasswordHasher.Generate(company.Password);
            if (claimRole != null)
                company.IdClaimRole = claimRole.Id;
            return await CompanyRepo.Save(company);
        }

        public async Task<bool> Save(UpdateCompanyModel CompanyDtoToSave)
        {
            CompanyDtoToSave.Password = PasswordHasher.Generate(CompanyDtoToSave.Password);
            return await CompanyRepo.Save(mapper.Map<Company>(CompanyDtoToSave));
        }
        public async Task<bool> Save(UpdateCompanyModel CompanyModelToSave, ClaimRole? claimRole)
        {
            Company company = mapper.Map<Company>(CompanyModelToSave);
            Company oldCompany = await CompanyRepo.GetById(company.Id);
            if (oldCompany == null)
                return false;
            company.DateOfEndPayment = company.DateOfEndPayment;
            company.DateOfRegister = company.DateOfRegister;
            company.Password = PasswordHasher.Generate(company.Password);
            if (claimRole != null)
                company.IdClaimRole = claimRole.Id;
            return await CompanyRepo.Save(company);
        }
    }
}
