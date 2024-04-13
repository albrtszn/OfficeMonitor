using AutoMapper;
using CRUD.implementation;
using DataBase.Repository.Models;
using OfficeMonitor.DTOs;
using OfficeMonitor.MiddleWares;
using OfficeMonitor.MiddleWares.Authorization;

namespace OfficeMonitor.Services
{
    public class ManagerService
    {
        private ManagerRepo ManagerRepo;
        private IMapper mapper;
        private JwtProvider jwt;
        public ManagerService(ManagerRepo _ManagerRepo, IMapper _mapper,
                              JwtProvider _jwt)
        {
            ManagerRepo = _ManagerRepo;
            mapper = _mapper;
            jwt = _jwt;
        }

        public async Task<bool> DeleteById(int id)
        {
            return await ManagerRepo.DeleteById(id);
        }

        public async Task<List<Manager>> GetAll()
        {
            return await ManagerRepo.GetAll();
        }

        public async Task<List<ManagerDto>> GetAllDtos()
        {
            List<ManagerDto> Managers = new List<ManagerDto>();
            List<Manager> list = await ManagerRepo.GetAll();
            list.ForEach(x => Managers.Add(mapper.Map<ManagerDto>(x)));
            return Managers;
        }

        public async Task<Manager?> GetById(int id)
        {
            return await ManagerRepo.GetById(id);
        }

        public async Task<ManagerDto> GetDtoById(int id)
        {
            return mapper.Map<ManagerDto>(await ManagerRepo.GetById(id));
        }

        public async Task<Manager?> GetByEmail(string email)
        {
            Manager? manager = (await ManagerRepo.GetAll())
                .FirstOrDefault(x => x != null && x.Login != null
                                && x.Login.Equals(email));
            return manager;
        }

        public async Task<string?> Login(string email, string password)
        {
            Manager? manager = await GetByEmail(email);
            if (manager == null)
                return null;
            if (PasswordHasher.Verify(password, manager.Password))
            {
                string token = jwt.GenerateToken(manager);
                return token;
            }
            else
            {
                return null;
            }
        }

        public async Task<bool> Save(Manager ManagerToSave)
        {
            return await ManagerRepo.Save(ManagerToSave);
        }


        public async Task<bool> Save(ManagerDto ManagerDtoToSave)
        {
            return await ManagerRepo.Save(mapper.Map<Manager>(ManagerDtoToSave));
        }
    }
}
