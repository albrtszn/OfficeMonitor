using AutoMapper;
using CRUD.implementation;
using DataBase.Repository.Models;
using OfficeMonitor.DTOs;
using Action = DataBase.Repository.Models.Action;

namespace OfficeMonitor.Services
{
    public class ActionService
    {
        private ActionRepo ActionRepo;
        private IMapper mapper;
        public ActionService(ActionRepo _ActionRepo, IMapper _mapper)
        {
            ActionRepo = _ActionRepo;
            mapper = _mapper;
        }

        public async Task<bool> DeleteById(int id)
        {
            return await ActionRepo.DeleteById(id);
        }

        public async Task<List<Action>> GetAll()
        {
            return await ActionRepo.GetAll();
        }

        public async Task<List<Action>> GetAllByEmployee(int employeeId)
        {
            return (await ActionRepo.GetAll()).Where(x=>x.IdEmployee!=null && x.IdEmployee.Equals(employeeId)).ToList();
        }

        public async Task<List<ActionDto>> GetAllDtos()
        {
            List<ActionDto> Actions = new List<ActionDto>();
            List<Action> list = await ActionRepo.GetAll();
            list.ForEach(x => Actions.Add(mapper.Map<ActionDto>(x)));
            return Actions;
        }

        public async Task<List<ActionDto>> GetAllDtosByEmployee(int employeeId)
        {
            List<ActionDto> Actions = new List<ActionDto>();
            List<Action> list = (await ActionRepo.GetAll()).Where(x => x.IdEmployee != null && x.IdEmployee.Equals(employeeId)).ToList();
            list.ForEach(x => Actions.Add(mapper.Map<ActionDto>(x)));
            return Actions;
        }

        public async Task<Action?> GetById(int id)
        {
            return await ActionRepo.GetById(id);
        }

        public async Task<ActionDto> GetDtoById(int id)
        {
            return mapper.Map<ActionDto>(await ActionRepo.GetById(id));
        }

        public async Task<bool> Save(Action ActionToSave)
        {
            return await ActionRepo.Save(ActionToSave);
        }

        public async Task<bool> Save(ActionDto ActionDtoToSave)
        {
            return await ActionRepo.Save(mapper.Map<Action>(ActionDtoToSave));
        }
    }
}
