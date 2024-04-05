using OfficeMonitor.DataBase.Models;
using OfficeMonitor.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CRUD.interfaces;
using Microsoft.EntityFrameworkCore;

namespace CRUD.implementation
{
    public class ProfileRepo : IntRepoInterface<Profile>
    {
        private AppDbContext context;
        public ProfileRepo(AppDbContext _context)
        {
            context = _context;
        }
        public async Task<bool> DeleteById(int id)
        {
            Profile? Profile = (await GetAll()).FirstOrDefault(x => x != null && x.Id.Equals(id));
            if (Profile == null)
                return false;
            context.Profiles.Remove(Profile);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Profile>> GetAll()
        {
            return await context.Profiles.ToListAsync();
        }

        public async Task<Profile?> GetById(int id)
        {
            return await context.Profiles.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<Profile?> GetTrackById(int id)
        {
            return await context.Profiles.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> Save(Profile entityToSave)
        {
            Profile? Profile = await GetTrackById(entityToSave.Id);
            //Profile? Profile = await context.Profiles.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(ProfileToSave.Id));
            if (Profile != null && entityToSave != null)
            {
                /*context.Profiles.Entry(ProfileToSave).State = EntityState.Detached;
                context.Set<Profile>().Update(ProfileToSave);*/
                Profile.Name = entityToSave.Name;
                Profile.IdCompany = entityToSave.IdCompany;
                Profile.IdDepartment = entityToSave.IdDepartment;

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                if (entityToSave != null)
                {
                    await context.Profiles.AddAsync(entityToSave);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
