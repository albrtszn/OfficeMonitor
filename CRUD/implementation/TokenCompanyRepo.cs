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
    public class TokenCompanyRepo : IntRepoInterface<TokenCompany>
    {
        private AppDbContext context;
        public TokenCompanyRepo(AppDbContext _context)
        {
            context = _context;
        }
        public async Task<bool> DeleteById(int id)
        {
            TokenCompany? TokenCompany = (await GetAll()).FirstOrDefault(x => x != null && x.Id.Equals(id));
            if (TokenCompany == null)
                return false;
            context.TokenCompanies.Remove(TokenCompany);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TokenCompany>> GetAll()
        {
            return await context.TokenCompanies.ToListAsync();
        }

        public async Task<TokenCompany?> GetById(int id)
        {
            return await context.TokenCompanies.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<TokenCompany?> GetByCompanyId(int idCompany)
        {
            return await context.TokenCompanies.FirstOrDefaultAsync(x => x.IdCompany != null && x.IdCompany.Equals(idCompany));
        }

        public static bool IsTokenExpired(TokenCompany? token)
        {
            //  todo ttl of Token
            if (token != null && token.DateOfCreation != null
                && (DateTime.Now - token.DateOfCreation.Value).TotalHours <= 24)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<TokenCompany?> GetTrackById(int id)
        {
            return await context.TokenCompanies.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> Save(TokenCompany entityToSave)
        {
            TokenCompany? TokenCompany = await GetTrackById(entityToSave.Id);
            //TokenCompany? TokenCompany = await context.TokenCompanys.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(TokenCompanyToSave.Id));
            if (TokenCompany != null && entityToSave != null)
            {
                /*context.TokenCompanys.Entry(TokenCompanyToSave).State = EntityState.Detached;
                context.Set<TokenCompany>().Update(TokenCompanyToSave);*/
                TokenCompany.IdCompany = entityToSave.IdCompany;
                TokenCompany.Token = entityToSave.Token;
                TokenCompany.DateOfCreation = entityToSave.DateOfCreation;

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                if (entityToSave != null)
                {
                    await context.TokenCompanies.AddAsync(entityToSave);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
