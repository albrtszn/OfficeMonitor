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
    public class CustomerRequestRepo : IntRepoInterface<CustomerRequest>
    {
        private AppDbContext context;
        public CustomerRequestRepo(AppDbContext _context)
        {
            context = _context;
        }
        public async Task<bool> DeleteById(int id)
        {
            CustomerRequest? CustomerRequest = (await GetAll()).FirstOrDefault(x => x != null && x.Id.Equals(id));
            if (CustomerRequest == null)
                return false;
            context.CustomerRequests.Remove(CustomerRequest);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<CustomerRequest>> GetAll()
        {
            return await context.CustomerRequests.ToListAsync();
        }

        public async Task<CustomerRequest?> GetById(int id)
        {
            return await context.CustomerRequests.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<CustomerRequest?> GetTrackById(int id)
        {
            return await context.CustomerRequests.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> Save(CustomerRequest entityToSave)
        {
            CustomerRequest? CustomerRequest = await GetTrackById(entityToSave.Id);
            //CustomerRequest? CustomerRequest = await context.CustomerRequests.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(CustomerRequestToSave.Id));
            if (CustomerRequest != null && entityToSave != null)
            {
                /*context.CustomerRequests.Entry(CustomerRequestToSave).State = EntityState.Detached;
                context.Set<CustomerRequest>().Update(CustomerRequestToSave);*/
                CustomerRequest.Email = entityToSave.Email;
                CustomerRequest.Name = entityToSave.Name;

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                if (entityToSave != null)
                {
                    await context.CustomerRequests.AddAsync(entityToSave);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
