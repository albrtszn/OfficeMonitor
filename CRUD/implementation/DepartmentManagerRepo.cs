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
    public class DepartmentManagerRepo : IntRepoInterface<DepartmentManager>
    {
        private AppDbContext context;
        public DepartmentManagerRepo(AppDbContext _context)
        {
            context = _context;
        }
        public async Task<bool> DeleteById(int id)
        {
            DepartmentManager? DepartmentManager = (await GetAll()).FirstOrDefault(x => x != null && x.Id.Equals(id));
            if (DepartmentManager == null)
                return false;
            context.DepartmentManagers.Remove(DepartmentManager);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<DepartmentManager>> GetAll()
        {
            return await context.DepartmentManagers.ToListAsync();
        }

        public async Task<DepartmentManager?> GetById(int id)
        {
            return await context.DepartmentManagers.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<DepartmentManager?> GetTrackById(int id)
        {
            return await context.DepartmentManagers.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> Save(DepartmentManager entityToSave)
        {
            DepartmentManager? DepartmentManager = await GetTrackById(entityToSave.Id);
            //DepartmentManager? DepartmentManager = await context.DepartmentManagers.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(DepartmentManagerToSave.Id));
            if (DepartmentManager != null && entityToSave != null)
            {
                /*context.DepartmentManagers.Entry(DepartmentManagerToSave).State = EntityState.Detached;
                context.Set<DepartmentManager>().Update(DepartmentManagerToSave);*/
                DepartmentManager.IdDepartment = entityToSave.IdDepartment;
                DepartmentManager.IdManager = entityToSave.IdManager;

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                if (entityToSave != null)
                {
                    await context.DepartmentManagers.AddAsync(entityToSave);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
