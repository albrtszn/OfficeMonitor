using DataBase.Repository.Models;
using DataBase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD.interfaces;
using Microsoft.EntityFrameworkCore;

namespace CRUD.implementation
{
    public class ManagerRepo : IntRepoInterface<Manager>
    {
        private AppDbContext context;
        public ManagerRepo(AppDbContext _context)
        {
            context = _context;
        }
        public async Task<bool> DeleteById(int id)
        {
            Manager? Manager = (await GetAll()).FirstOrDefault(x => x != null && x.Id.Equals(id));
            if (Manager == null)
                return false;
            context.Managers.Remove(Manager);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Manager>> GetAll()
        {
            return await context.Managers.ToListAsync();
        }

        public async Task<Manager?> GetById(int id)
        {
            return await context.Managers.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<Manager?> GetTrackById(int id)
        {
            return await context.Managers.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> Save(Manager entityToSave)
        {
            Manager? Manager = await GetTrackById(entityToSave.Id);
            //Manager? Manager = await context.Managers.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(ManagerToSave.Id));
            if (Manager != null && entityToSave != null)
            {
                /*context.Managers.Entry(ManagerToSave).State = EntityState.Detached;
                context.Set<Manager>().Update(ManagerToSave);*/
                Manager.IdClaimRole = entityToSave.IdClaimRole;
                Manager.Name = entityToSave.Name;
                Manager.Surname = entityToSave.Surname;
                Manager.Patronamic = entityToSave.Patronamic;
                Manager.Login = entityToSave.Login;
                Manager.Password = entityToSave.Password;
                Manager.IdProfile = entityToSave.IdProfile;

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                if (entityToSave != null)
                {
                    await context.Managers.AddAsync(entityToSave);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
