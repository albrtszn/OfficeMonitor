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
    public class TokenAdminRepo : IntRepoInterface<TokenAdmin>
    {
        private AppDbContext context;
        public TokenAdminRepo(AppDbContext _context)
        {
            context = _context;
        }
        public async Task<bool> DeleteById(int id)
        {
            TokenAdmin? TokenAdmin = (await GetAll()).FirstOrDefault(x => x != null && x.Id.Equals(id));
            if (TokenAdmin == null)
                return false;
            context.TokenAdmins.Remove(TokenAdmin);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TokenAdmin>> GetAll()
        {
            return await context.TokenAdmins.ToListAsync();
        }

        public async Task<TokenAdmin?> GetById(int id)
        {
            return await context.TokenAdmins.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<TokenAdmin?> GetTrackById(int id)
        {
            return await context.TokenAdmins.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> Save(TokenAdmin entityToSave)
        {
            TokenAdmin? TokenAdmin = await GetTrackById(entityToSave.Id);
            //TokenAdmin? TokenAdmin = await context.TokenAdmins.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(TokenAdminToSave.Id));
            if (TokenAdmin != null && entityToSave != null)
            {
                /*context.TokenAdmins.Entry(TokenAdminToSave).State = EntityState.Detached;
                context.Set<TokenAdmin>().Update(TokenAdminToSave);*/
                TokenAdmin.IdAdmin = entityToSave.IdAdmin;
                TokenAdmin.Token = entityToSave.Token;
                TokenAdmin.DateOfCreation = entityToSave.DateOfCreation;

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                if (entityToSave != null)
                {
                    await context.TokenAdmins.AddAsync(entityToSave);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
