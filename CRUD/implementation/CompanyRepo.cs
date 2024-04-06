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
    public class CompanyRepo : IntRepoInterface<Company>
    {
        private AppDbContext context;
        public CompanyRepo(AppDbContext _context)
        {
            context = _context;
        }
        public async Task<bool> DeleteById(int id)
        {
            Company? Company = (await GetAll()).FirstOrDefault(x => x != null && x.Id.Equals(id));
            if (Company == null)
                return false;
            context.Companies.Remove(Company);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Company>> GetAll()
        {
            return await context.Companies.ToListAsync();
        }

        public async Task<Company?> GetById(int id)
        {
            return await context.Companies.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<Company?> GetTrackById(int id)
        {
            return await context.Companies.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> Save(Company entityToSave)
        {
            Company? Company = await GetTrackById(entityToSave.Id);
            //Company? Company = await context.Companys.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(CompanyToSave.Id));
            if (Company != null && entityToSave != null)
            {
                /*context.Companys.Entry(CompanyToSave).State = EntityState.Detached;
                context.Set<Company>().Update(CompanyToSave);*/
                Company.Login = entityToSave.Login;
                Company.Password = entityToSave.Password;
                Company.Name = entityToSave.Name;
                Company.Description = entityToSave.Description;
                Company.IdPlan = entityToSave.IdPlan;
                Company.Balance = entityToSave.Balance;
                Company.IsActive = entityToSave.IsActive;
                Company.IsBanned = entityToSave.IsBanned;
                Company.DateOfRegister = entityToSave.DateOfRegister;
                Company.DateOfEndPayment = entityToSave.DateOfEndPayment;

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                if (entityToSave != null)
                {
                    await context.Companies.AddAsync(entityToSave);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
