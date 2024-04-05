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
    public class WorkTimeRepo : IntRepoInterface<WorkTime>
    {
        private AppDbContext context;
        public WorkTimeRepo(AppDbContext _context)
        {
            context = _context;
        }
        public async Task<bool> DeleteById(int id)
        {
            WorkTime? WorkTime = (await GetAll()).FirstOrDefault(x => x != null && x.Id.Equals(id));
            if (WorkTime == null)
                return false;
            context.WorkTimes.Remove(WorkTime);
            await context.SaveChangesAsync();
            return true;
        }

        public async Task<List<WorkTime>> GetAll()
        {
            return await context.WorkTimes.ToListAsync();
        }

        public async Task<WorkTime?> GetById(int id)
        {
            return await context.WorkTimes.FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<WorkTime?> GetTrackById(int id)
        {
            return await context.WorkTimes.AsTracking().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public async Task<bool> Save(WorkTime entityToSave)
        {
            WorkTime? WorkTime = await GetTrackById(entityToSave.Id);
            //WorkTime? WorkTime = await context.WorkTimes.AsNoTracking().FirstOrDefaultAsync(x => x.Id.Equals(WorkTimeToSave.Id));
            if (WorkTime != null && entityToSave != null)
            {
                /*context.WorkTimes.Entry(WorkTimeToSave).State = EntityState.Detached;
                context.Set<WorkTime>().Update(WorkTimeToSave);*/
                WorkTime.IdDepartment = entityToSave.IdDepartment;
                WorkTime.StartTime = entityToSave.StartTime;
                WorkTime.EndTime = entityToSave.EndTime;

                await context.SaveChangesAsync();
                return true;
            }
            else
            {
                if (entityToSave != null)
                {
                    await context.WorkTimes.AddAsync(entityToSave);
                    await context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
    }
}
