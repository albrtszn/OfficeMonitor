using CRUD.interfaces;
using Microsoft.EntityFrameworkCore;
using OfficeMonitor.DataBase;
using OfficeMonitor.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyAction = OfficeMonitor.DataBase.Models.Action;

namespace CRUD.implementation
{
    public class ActionRepo : IntRepoInterface<MyAction>
    {
        private AppDbContext context;
        public ActionRepo(AppDbContext _context)
        {
            context = _context;
        }
        public async Task<bool> DeleteById(int id)
        {
            MyAction? action = (await GetAll()).FirstOrDefault(x=> x!=null && x.Id.Equals(id));
            if (action == null) 
                return false;
            context.Actions.Remove(action);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<MyAction>> GetAll()
        {
            return await context.Actions.ToListAsync();
        }

        public async Task<MyAction?> GetById(int id)
        {
            return await context.Actions.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<MyAction?> GetTrackById(int id)
        {
            return await context.Actions.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> Save(MyAction entityToSave)
        {
            MyAction? action = await GetTrackById(entityToSave.Id);
            //Admin? admin = await context.Admins.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(AdminToSave.Id));
            if (action != null)
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
