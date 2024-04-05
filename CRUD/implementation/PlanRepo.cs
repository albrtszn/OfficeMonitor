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
    public class PlanRepo : IntRepoInterface<Plan>
    {
        private AppDbContext context;
        public PlanRepo(AppDbContext _context)
        {
            context = _context;
        }
        public async Task<bool> DeleteById(int id)
        {
            Plan? Plan = (await GetAll()).FirstOrDefault(x => x != null && x.Id.Equals(id));
            if (Plan == null)
                return false;
            context.Plans.Remove(Plan);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Plan>> GetAll()
        {
            return await context.Plans.ToListAsync();
        }

        public async Task<Plan?> GetById(int id)
        {
            return await context.Plans.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<Plan?> GetTrackById(int id)
        {
            return await context.Plans.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> Save(Plan entityToSave)
        {
            Plan? Plan = await GetTrackById(entityToSave.Id);
            //Plan? Plan = await context.Plans.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(PlanToSave.Id));
            if (Plan != null && entityToSave != null)
            {
                /*context.Plans.Entry(PlanToSave).State = EntityState.Detached;
                context.Set<Plan>().Update(PlanToSave);*/
                Plan.Name = entityToSave.Name;
                Plan.Description = entityToSave.Description;
                Plan.MonthCost = entityToSave.MonthCost;
                Plan.Yearcost = entityToSave.Yearcost;
                Plan.CountOfEmployees = entityToSave.CountOfEmployees;

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                if (entityToSave != null)
                {
                    await context.Plans.AddAsync(entityToSave);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
