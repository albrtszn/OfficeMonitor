using AutoMapper;
using CRUD.implementation;
using DataBase.Repository.Models;
using OfficeMonitor.DTOs;
using OfficeMonitor.MiddleWares;
using OfficeMonitor.MiddleWares.Authorization;
using OfficeMonitor.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OfficeMonitor.Services
{
    public class AdminService
    {
        private AdminRepo AdminRepo;
        private ClaimRoleRepo ClaimRoleRepo;
        private TokenAdminRepo TokenAdminRepo;
        private IMapper mapper;
        private JwtProvider jwt;
        public AdminService(AdminRepo _AdminRepo, IMapper _mapper,
                            JwtProvider _jwt, ClaimRoleRepo _ClaimRoleRepo, 
                            TokenAdminRepo _TokenAdminRepo)
        {
            AdminRepo = _AdminRepo;
            mapper = _mapper;
            jwt = _jwt;
            ClaimRoleRepo = _ClaimRoleRepo;
            TokenAdminRepo = _TokenAdminRepo;
        }

        public async Task<bool> DeleteById(int id)
        {
            return await AdminRepo.DeleteById(id);
        }

        public async Task<List<Admin>> GetAll()
        {
            return await AdminRepo.GetAll();
        }

        public async Task<List<AdminDto>> GetAllDtos()
        {
            List<AdminDto> Admins = new List<AdminDto>();
            List<Admin> list = await AdminRepo.GetAll();
            list.ForEach(x => Admins.Add(mapper.Map<AdminDto>(x)));
            return Admins;
        }

        public async Task<Admin?> GetById(int id)
        {
            return await AdminRepo.GetById(id);
        }

        public async Task<Admin?> GetByEmail(string email)
        {
            Admin? admin = (await AdminRepo.GetAll())
                .FirstOrDefault(x => x != null && x.Login != null
                                && x.Login.Equals(email));
            return admin;
        }

        public async Task<string?> Login(string email, string password)
        {
            Admin? admin = await GetByEmail(email);
            if (admin == null)
                return null;
            if (PasswordHasher.Verify(password, admin.Password))
            {
                TokenAdmin? tokenCompany = await TokenAdminRepo.GetByAdminId(admin.Id);
                if (!TokenAdminRepo.IsTokenExpired(tokenCompany))
                {
                    return tokenCompany.Token;
                }
                else
                {
                    ClaimRole? role = (await ClaimRoleRepo.GetById(admin.IdClaimRole.Value));
                    string token = jwt.GenerateToken(admin, role != null ? role.Name : "COMPANY");
                    await TokenAdminRepo.Save(new TokenAdmin
                    {
                        IdAdmin = admin.Id,
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
        public async Task<AdminDto> GetDtoById(int id)
        {
            return mapper.Map<AdminDto>(await AdminRepo.GetById(id));
        }

        public async Task<bool> Save(Admin AdminToSave)
        {
            AdminToSave.Password = PasswordHasher.Generate(AdminToSave.Password);
            return await AdminRepo.Save(AdminToSave);
        }

        public async Task<bool> Save(AdminDto AdminDtoToSave)
        {
            AdminDtoToSave.Password = PasswordHasher.Generate(AdminDtoToSave.Password);
            return await AdminRepo.Save(mapper.Map<Admin>(AdminDtoToSave));
        }        
        
        public async Task<bool> Save(AddAdminModel AdminToSave)
        {
            AdminToSave.Password = PasswordHasher.Generate(AdminToSave.Password);
            return await AdminRepo.Save(mapper.Map<Admin>(AdminToSave));
        }        
        
        public async Task<bool> Save(AddAdminModel AdminToSave, ClaimRole? claimRole)
        {
            Admin admin = mapper.Map<Admin>(AdminToSave);
            admin.Password = PasswordHasher.Generate(admin.Password);
            if (claimRole != null)
                admin.IdClaimRole = claimRole.Id;
            return await AdminRepo.Save(admin);
        }
    }
}
