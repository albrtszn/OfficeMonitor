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
    public class TypeAppRepo : IntRepoInterface<TypeApp>
    {
        private AppDbContext context;
        public TypeAppRepo(AppDbContext _context)
        {
            context = _context;
        }
        public async Task<bool> DeleteById(int id)
        {
            TypeApp? TypeApp = (await GetAll()).FirstOrDefault(x => x != null && x.Id.Equals(id));
            if (TypeApp == null)
                return false;
            context.TypeApps.Remove(TypeApp);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TypeApp>> GetAll()
        {
            return await context.TypeApps.ToListAsync();
        }

        public async Task<TypeApp?> GetById(int id)
        {
            return await context.TypeApps.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<TypeApp?> GetTrackById(int id)
        {
            return await context.TypeApps.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> Save(TypeApp entityToSave)
        {
            TypeApp? TypeApp = await GetTrackById(entityToSave.Id);
            //TypeApp? TypeApp = await context.TypeApps.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(TypeAppToSave.Id));
            if (TypeApp != null && entityToSave != null)
            {
                /*context.TypeApps.Entry(TypeAppToSave).State = EntityState.Detached;
                context.Set<TypeApp>().Update(TypeAppToSave);*/
                TypeApp.Name = entityToSave.Name;
                TypeApp.Description = entityToSave.Description;

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                if (entityToSave != null)
                {
                    await context.TypeApps.AddAsync(entityToSave);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
