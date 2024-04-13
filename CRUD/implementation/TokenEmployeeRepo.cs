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
    public class TokenEmployeeRepo : IntRepoInterface<TokenEmployee>
    {
        private AppDbContext context;
        public TokenEmployeeRepo(AppDbContext _context)
        {
            context = _context;
        }
        public async Task<bool> DeleteById(int id)
        {
            TokenEmployee? TokenEmployee = (await GetAll()).FirstOrDefault(x => x != null && x.Id.Equals(id));
            if (TokenEmployee == null)
                return false;
            context.TokenEmployees.Remove(TokenEmployee);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<TokenEmployee>> GetAll()
        {
            return await context.TokenEmployees.ToListAsync();
        }

        public async Task<TokenEmployee?> GetById(int id)
        {
            return await context.TokenEmployees.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<TokenEmployee?> GetTrackById(int id)
        {
            return await context.TokenEmployees.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> Save(TokenEmployee entityToSave)
        {
            TokenEmployee? TokenEmployee = await GetTrackById(entityToSave.Id);
            //TokenEmployee? TokenEmployee = await context.TokenEmployees.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(TokenEmployeeToSave.Id));
            if (TokenEmployee != null && entityToSave != null)
            {
                /*context.TokenEmployees.Entry(TokenEmployeeToSave).State = EntityState.Detached;
                context.Set<TokenEmployee>().Update(TokenEmployeeToSave);*/
                TokenEmployee.IdEmployee = entityToSave.IdEmployee;
                TokenEmployee.IdClaimRole = entityToSave.IdClaimRole;
                TokenEmployee.Token = entityToSave.Token;
                TokenEmployee.DateOfCreation = entityToSave.DateOfCreation;

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                if (entityToSave != null)
                {
                    await context.TokenEmployees.AddAsync(entityToSave);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
