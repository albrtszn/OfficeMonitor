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
    public class EmployeeRepo : IntRepoInterface<Employee>
    {
        private AppDbContext context;
        public EmployeeRepo(AppDbContext _context)
        {
            context = _context;
        }
        public async Task<bool> DeleteById(int id)
        {
            Employee? Employee = (await GetAll()).FirstOrDefault(x => x != null && x.Id.Equals(id));
            if (Employee == null)
                return false;
            context.Employees.Remove(Employee);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Employee>> GetAll()
        {
            return await context.Employees.ToListAsync();
        }

        public async Task<Employee?> GetById(int id)
        {
            return await context.Employees.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<Employee?> GetTrackById(int id)
        {
            return await context.Employees.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> Save(Employee entityToSave)
        {
            Employee? Employee = await GetTrackById(entityToSave.Id);
            //Employee? Employee = await context.Employees.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(EmployeeToSave.Id));
            if (Employee != null && entityToSave != null)
            {
                /*context.Employees.Entry(EmployeeToSave).State = EntityState.Detached;
                context.Set<Employee>().Update(EmployeeToSave);*/
                Employee.Name = entityToSave.Name;
                Employee.Surname = entityToSave.Surname;
                Employee.Patronamic = entityToSave.Patronamic;
                Employee.Login = entityToSave.Login;
                Employee.Password = entityToSave.Password;
                Employee.IdProfile = entityToSave.IdProfile;

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                if (entityToSave != null)
                {
                    await context.Employees.AddAsync(entityToSave);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
