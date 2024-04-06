using CRUD.interfaces;
using Microsoft.EntityFrameworkCore;
using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase.Repository;
using Action = DataBase.Repository.Models.Action;

namespace CRUD.implementation
{
    public class ActionRepo : IntRepoInterface<Action>
    {
        private AppDbContext context;
        public ActionRepo(AppDbContext _context)
        {
            context = _context;
        }
        public async Task<bool> DeleteById(int id)
        {
            Action? action = (await GetAll()).FirstOrDefault(x=> x!=null && x.Id.Equals(id));
            if (action == null) 
                return false;
            context.Actions.Remove(action);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Action>> GetAll()
        {
            return await context.Actions.ToListAsync();
        }

        public async Task<Action?> GetById(int id)
        {
            return await context.Actions.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<Action?> GetTrackById(int id)
        {
            return await context.Actions.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> Save(Action entityToSave)
        {
            Action? action = await GetTrackById(entityToSave.Id);
            //Admin? admin = await context.Admins.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(AdminToSave.Id));
            if (action != null && entityToSave != null)
            {
                /*context.Admins.Entry(AdminToSave).State = EntityState.Detached;
                context.Set<Admin>().Update(AdminToSave);*/
                action.IdEmployee = entityToSave.IdEmployee;
                action.Date = entityToSave.Date;
                action.IdApp = entityToSave.IdApp;
                action.StartTime = entityToSave.StartTime;
                action.EndTime = entityToSave.EndTime;

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                if (entityToSave != null)
                {
                    await context.Actions.AddAsync(entityToSave);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
