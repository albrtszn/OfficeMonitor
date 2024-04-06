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
    public class DepartmentAppRepo : IntRepoInterface<DepartmentApp>
    {
        private AppDbContext context;
        public DepartmentAppRepo(AppDbContext _context)
        {
            context = _context;
        }
        public async Task<bool> DeleteById(int id)
        {
            DepartmentApp? DepartmentApp = (await GetAll()).FirstOrDefault(x => x != null && x.Id.Equals(id));
            if (DepartmentApp == null)
                return false;
            context.DepartmentApps.Remove(DepartmentApp);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<DepartmentApp>> GetAll()
        {
            return await context.DepartmentApps.ToListAsync();
        }

        public async Task<DepartmentApp?> GetById(int id)
        {
            return await context.DepartmentApps.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<DepartmentApp?> GetTrackById(int id)
        {
            return await context.DepartmentApps.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> Save(DepartmentApp entityToSave)
        {
            DepartmentApp? DepartmentApp = await GetTrackById(entityToSave.Id);
            //DepartmentApp? DepartmentApp = await context.DepartmentApps.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(DepartmentAppToSave.Id));
            if (DepartmentApp != null && entityToSave != null)
            {
                /*context.DepartmentApps.Entry(DepartmentAppToSave).State = EntityState.Detached;
                context.Set<DepartmentApp>().Update(DepartmentAppToSave);*/
                DepartmentApp.IdDepartment = entityToSave.IdDepartment;
                DepartmentApp.IdApp = entityToSave.IdApp;

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                if (entityToSave != null)
                {
                    await context.DepartmentApps.AddAsync(entityToSave);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
