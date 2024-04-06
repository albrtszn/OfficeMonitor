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
    public class AppRepo : IntRepoInterface<App>
    {
        private AppDbContext context;
        public AppRepo(AppDbContext _context)
        {
            context = _context;
        }
        public async Task<bool> DeleteById(int id)
        {
            App? App = (await GetAll()).FirstOrDefault(x => x != null && x.Id.Equals(id));
            if (App == null)
                return false;
            context.Apps.Remove(App);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<App>> GetAll()
        {
            return await context.Apps.ToListAsync();
        }

        public async Task<App?> GetById(int id)
        {
            return await context.Apps.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<App?> GetTrackById(int id)
        {
            return await context.Apps.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> Save(App entityToSave)
        {
            App? App = await GetTrackById(entityToSave.Id);
            //App? App = await context.Apps.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(AppToSave.Id));
            if (App != null && entityToSave != null)
            {
                /*context.Apps.Entry(AppToSave).State = EntityState.Detached;
                context.Set<App>().Update(AppToSave);*/
                App.Name = entityToSave.Name;
                App.IdTypeApp = entityToSave.IdTypeApp;

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                if (entityToSave != null)
                {
                    await context.Apps.AddAsync(entityToSave);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
