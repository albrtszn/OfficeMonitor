using CRUD.interfaces;
using DataBase.Repository.Models;
using DataBase.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CRUD.implementation
{
    public class ClaimRoleRepo : IntRepoInterface<ClaimRole>
    {
        private AppDbContext context;
        public ClaimRoleRepo(AppDbContext _context)
        {
            context = _context;
        }
        public async Task<bool> DeleteById(int id)
        {
            ClaimRole? ClaimRole = (await GetAll()).FirstOrDefault(x => x != null && x.Id.Equals(id));
            if (ClaimRole == null)
                return false;
            context.ClaimRoles.Remove(ClaimRole);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<ClaimRole>> GetAll()
        {
            return await context.ClaimRoles.ToListAsync();
        }

        public async Task<ClaimRole?> GetById(int id)
        {
            return await context.ClaimRoles.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<ClaimRole?> GetTrackById(int id)
        {
            return await context.ClaimRoles.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> Save(ClaimRole entityToSave)
        {
            ClaimRole? ClaimRole = await GetTrackById(entityToSave.Id);
            //ClaimRole? ClaimRole = await context.ClaimRoles.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(ClaimRoleToSave.Id));
            if (ClaimRole != null && entityToSave != null)
            {
                /*context.ClaimRoles.Entry(ClaimRoleToSave).State = EntityState.Detached;
                context.Set<ClaimRole>().Update(ClaimRoleToSave);*/
                ClaimRole.Name = entityToSave.Name;

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                if (entityToSave != null)
                {
                    await context.ClaimRoles.AddAsync(entityToSave);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
