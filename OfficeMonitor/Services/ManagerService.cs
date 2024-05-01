using AutoMapper;
using CRUD.implementation;
using DataBase.Repository.Models;
using OfficeMonitor.DTOs;
using OfficeMonitor.MiddleWares;
using OfficeMonitor.MiddleWares.Authorization;
using OfficeMonitor.Models.Manager;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace OfficeMonitor.Services
{
    public class ManagerService
    {
        private ManagerRepo ManagerRepo;
        private ClaimRoleRepo ClaimRoleRepo;
        private TokenManagerRepo TokenManagerRepo;
        private IMapper mapper;
        private JwtProvider jwt;
        public ManagerService(ManagerRepo _ManagerRepo, IMapper _mapper,
                              JwtProvider _jwt, ClaimRoleRepo _ClaimRoleRepo, 
                              TokenManagerRepo tokenManagerRepo)
        {
            ManagerRepo = _ManagerRepo;
            mapper = _mapper;
            jwt = _jwt;
            ClaimRoleRepo = _ClaimRoleRepo;
            TokenManagerRepo = tokenManagerRepo;
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
                TokenManager? tokenManager = await TokenManagerRepo.GetByManagerId(manager.Id);
                //  todo token ttl
                if (tokenManager != null &&!TokenManagerRepo.IsTokenExpired(tokenManager))
                {
                    return tokenManager.Token;
                }
                else
                {
                    if(tokenManager != null)
                        await TokenManagerRepo.DeleteById(tokenManager.Id);
                    ClaimRole role = (await ClaimRoleRepo.GetById(manager.IdClaimRole.Value));
                    string token = jwt.GenerateToken(manager, role != null ? role.Name : "MANAGER");
                    await TokenManagerRepo.Save(new TokenManager
                    {
                        IdManager = manager.Id,
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

        public async Task<bool> Save(Manager ManagerToSave)
        {
            ManagerToSave.Password = PasswordHasher.Generate(ManagerToSave.Password);
            return await ManagerRepo.Save(ManagerToSave);
        }


        public async Task<bool> Save(ManagerDto ManagerDtoToSave)
        {
            ManagerDtoToSave.Password = PasswordHasher.Generate(ManagerDtoToSave.Password);
            return await ManagerRepo.Save(mapper.Map<Manager>(ManagerDtoToSave));
        }

        public async Task<bool> Save(AddManagerModel ManagerDtoToSave)
        {
            ManagerDtoToSave.Password = PasswordHasher.Generate(ManagerDtoToSave.Password);
            return await ManagerRepo.Save(mapper.Map<Manager>(ManagerDtoToSave));
        }

        public async Task<bool> Save(AddManagerModel ManagerDtoToSave, ClaimRole? claimRole)
        {
            Manager manager = mapper.Map<Manager>(ManagerDtoToSave);
            manager.Password = PasswordHasher.Generate(manager.Password);
            if (claimRole != null)
                manager.IdClaimRole = claimRole.Id;
            return await ManagerRepo.Save(manager);
        }

        public async Task<bool> Save(UpdateManagerModel ManagerModelToSave, ClaimRole? claimRole)
        {
            Manager manager = mapper.Map<Manager>(ManagerModelToSave);
            manager.Password = PasswordHasher.Generate(manager.Password);
            if (claimRole != null)
                manager.IdClaimRole = claimRole.Id;
            return await ManagerRepo.Save(manager);
        }
    }
}
