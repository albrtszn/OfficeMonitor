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
    public class DepartmentRepo : IntRepoInterface<Department>
    {
        private AppDbContext context;
        public DepartmentRepo(AppDbContext _context)
        {
            context = _context;
        }
        public async Task<bool> DeleteById(int id)
        {
            Department? Department = (await GetAll()).FirstOrDefault(x => x != null && x.Id.Equals(id));
            if (Department == null)
                return false;
            context.Departments.Remove(Department);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Department>> GetAll()
        {
            return await context.Departments.ToListAsync();
        }

        public async Task<Department?> GetById(int id)
        {
            return await context.Departments.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<Department?> GetTrackById(int id)
        {
            return await context.Departments.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> Save(Department entityToSave)
        {
            Department? Department = await GetTrackById(entityToSave.Id);
            //Department? Department = await context.Departments.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(DepartmentToSave.Id));
            if (Department != null && entityToSave != null)
            {
                /*context.Departments.Entry(DepartmentToSave).State = EntityState.Detached;
                context.Set<Department>().Update(DepartmentToSave);*/
                Department.Name = entityToSave.Name;
                Department.Description = entityToSave.Description;
                Department.IdCompany = entityToSave.IdCompany;

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                if (entityToSave != null)
                {
                    await context.Departments.AddAsync(entityToSave);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
