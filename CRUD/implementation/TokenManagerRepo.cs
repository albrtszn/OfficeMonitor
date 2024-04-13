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
    public class TokenManagerRepo : IntRepoInterface<TokenManager>
    {
        private AppDbContext context;
        public TokenManagerRepo(AppDbContext _context)
        {
            context = _context;
        }
        public async Task<bool> DeleteById(int id)
        {
            TokenManager? TokenManager = (await GetAll()).FirstOrDefault(x => x != null && x.Id.Equals(id));
            if (TokenManager == null)
                return false;
            context.TokenManagers.Remove(TokenManager);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TokenManager>> GetAll()
        {
            return await context.TokenManagers.ToListAsync();
        }

        public async Task<TokenManager?> GetById(int id)
        {
            return await context.TokenManagers.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<TokenManager?> GetTrackById(int id)
        {
            return await context.TokenManagers.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> Save(TokenManager entityToSave)
        {
            TokenManager? TokenManager = await GetTrackById(entityToSave.Id);
            //TokenManager? TokenManager = await context.TokenManagers.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(TokenManagerToSave.Id));
            if (TokenManager != null && entityToSave != null)
            {
                /*context.TokenManagers.Entry(TokenManagerToSave).State = EntityState.Detached;
                context.Set<TokenManager>().Update(TokenManagerToSave);*/
                TokenManager.IdManager = entityToSave.IdManager;
                TokenManager.Token = entityToSave.Token;
                TokenManager.DateOfCreation = entityToSave.DateOfCreation;

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                if (entityToSave != null)
                {
                    await context.TokenManagers.AddAsync(entityToSave);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
