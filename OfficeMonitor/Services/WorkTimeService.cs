using AutoMapper;
using CRUD.implementation;
using DataBase.Repository.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeMonitor.DTOs;
using OfficeMonitor.Models.WorkTime;

namespace OfficeMonitor.Services
{
    public class WorkTimeService
    {
        private WorkTimeRepo WorkTimeRepo;
        private IMapper mapper;
        public WorkTimeService(WorkTimeRepo _WorkTimeRepo, IMapper _mapper)
        {
            WorkTimeRepo = _WorkTimeRepo;
            mapper = _mapper;
        }

        public async Task<bool> DeleteById(int id)
        {
            return await WorkTimeRepo.DeleteById(id);
        }

        public async Task<List<WorkTime>> GetAll()
        {
            return await WorkTimeRepo.GetAll();
        }

        public async Task<List<WorkTimeDto>> GetAllDtos()
        {
            List<WorkTimeDto> WorkTimes = new List<WorkTimeDto>();
            List<WorkTime> list = await WorkTimeRepo.GetAll();
            list.ForEach(x => WorkTimes.Add(mapper.Map<WorkTimeDto>(x)));
            return WorkTimes;
        }

        public async Task<WorkTime?> GetById(int id)
        {
            return await WorkTimeRepo.GetById(id);
        }

        public async Task<WorkTimeDto?> GetDtoByDepartmentId(int id)
        {
            WorkTime? workTime = (await WorkTimeRepo.GetAll()).FirstOrDefault(x=>x!=null && x.IdDepartment!= null 
                                                                                 && x.IdDepartment.Equals(id));
            if (workTime == null)
                return null;
            return mapper.Map<WorkTimeDto>(workTime);
        }

        public async Task<WorkTimeDto> GetDtoById(int id)
        {
            return mapper.Map<WorkTimeDto>(await WorkTimeRepo.GetById(id));
        }

        public async Task<bool> Save(WorkTime WorkTimeToSave)
        {
            return await WorkTimeRepo.Save(WorkTimeToSave);
        }

        public async Task<bool> Save(WorkTimeDto WorkTimeDtoToSave)
        {
            return await WorkTimeRepo.Save(mapper.Map<WorkTime>(WorkTimeDtoToSave));
        }

        public async Task<bool> Save(AddWorkTimeModel WorkTimeModelToSave)
        {
            return await WorkTimeRepo.Save(mapper.Map<WorkTime>(WorkTimeModelToSave));
        }
        public async Task<bool> Save(UpdateWorkTimeModel WorkTimeModelToSave)
        {
            return await WorkTimeRepo.Save(mapper.Map<WorkTime>(WorkTimeModelToSave));
        }
    }
}
