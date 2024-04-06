﻿using AutoMapper;
using CRUD.implementation;
using OfficeMonitor.DataBase.Models;
using OfficeMonitor.DTOs;

namespace OfficeMonitor.Services
{
    public class PlanService
    {
        private PlanRepo PlanRepo;
        private IMapper mapper;
        public PlanService(PlanRepo _PlanRepo, IMapper _mapper)
        {
            PlanRepo = _PlanRepo;
            mapper = _mapper;
        }

        public async Task<bool> DeleteById(int id)
        {
            return await PlanRepo.DeleteById(id);
        }

        public async Task<List<Plan>> GetAll()
        {
            return await PlanRepo.GetAll();
        }

        public async Task<List<PlanDto>> GetAllDtos()
        {
            List<PlanDto> Plans = new List<PlanDto>();
            List<Plan> list = await PlanRepo.GetAll();
            list.ForEach(x => Plans.Add(mapper.Map<PlanDto>(x)));
            return Plans;
        }

        public async Task<Plan?> GetById(int id)
        {
            return await PlanRepo.GetById(id);
        }

        public async Task<PlanDto> GetDtoById(int id)
        {
            return mapper.Map<PlanDto>(await PlanRepo.GetById(id));
        }

        public async Task<bool> Save(Plan PlanToSave)
        {
            return await PlanRepo.Save(PlanToSave);
        }

        public async Task<bool> Save(PlanDto PlanDtoToSave)
        {
            return await PlanRepo.Save(mapper.Map<Plan>(PlanDtoToSave));
        }
    }
}