using CRUD.interfaces;
using Microsoft.EntityFrameworkCore;
using DataBase.Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataBase.Repository;


namespace CRUD.implementation
{
    public class AdminRepo : IntRepoInterface<Admin>
    {
        private AppDbContext context;
        public AdminRepo(AppDbContext _context)
        {
            context = _context;
        }
        public async Task<bool> DeleteById(int id)
        {
            Admin? admin = (await GetAll()).FirstOrDefault(x => x != null && x.Id.Equals(id));
            if (admin == null)
                return false;
            context.Admins.Remove(admin);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Admin>> GetAll()
        {
            return await context.Admins.ToListAsync();
        }

        public async Task<Admin?> GetById(int id)
        {
            return await context.Admins.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<Admin?> GetTrackById(int id)
        {
            return await context.Admins.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> Save(Admin entityToSave)
        {
            Admin? Admin = await GetTrackById(entityToSave.Id);
            //Admin? admin = await context.Admins.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(AdminToSave.Id));
            if (Admin != null && entityToSave != null)
            {
                /*context.Admins.Entry(AdminToSave).State = EntityState.Detached;
                context.Set<Admin>().Update(AdminToSave);*/
                Admin.Name = entityToSave.Name;
                Admin.IdClaimRole = entityToSave.IdClaimRole;
                Admin.Surname = entityToSave.Surname;
                Admin.Patronamic = entityToSave.Patronamic;
                Admin.Login = entityToSave.Login;
                Admin.Password = entityToSave.Password;

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                if (entityToSave != null)
                {
                    await context.Admins.AddAsync(entityToSave);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
