using AutoMapper;
using CRUD.implementation;
using DataBase.Repository.Models;
using OfficeMonitor.DTOs;
using OfficeMonitor.MiddleWares;
using OfficeMonitor.MiddleWares.Authorization;

namespace OfficeMonitor.Services
{
    public class AdminService
    {
        private AdminRepo AdminRepo;
        private IMapper mapper;
        private JwtProvider jwt;
        public AdminService(AdminRepo _AdminRepo, IMapper _mapper,
                            JwtProvider _jwt)
        {
            AdminRepo = _AdminRepo;
            mapper = _mapper;
            jwt = _jwt;
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
                string token = jwt.GenerateToken(admin);
                return token;
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
            return await AdminRepo.Save(AdminToSave);
        }

        public async Task<bool> Save(AdminDto AdminDtoToSave)
        {
            return await AdminRepo.Save(mapper.Map<Admin>(AdminDtoToSave));
        }
    }
}
