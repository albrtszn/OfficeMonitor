using CRUD.implementation;
using OfficeMonitor.DataBase.Models;
using OfficeMonitor.DTOs;
using Action = OfficeMonitor.DataBase.Models.Action;

namespace OfficeMonitor.Services
{
    public class ActionService
    {
        /*private ActionRepo ActionRepo;
        public ActionService(ActionRepo _ActionRepo)
        {
            this.ActionRepo = _ActionRepo;
        }

        public async Task<bool> DeleteActionById(int id)
        {
            return await ActionRepo.DeleteById(id);
        }

        public async Task<List<Action>> GetAllActions()
        {
            return await ActionRepo.GetAll();
        }

        public async Task<List<ActionDto>> GetAllActionDtos()
        {
            List<ActionDto> Actions = new List<ActionDto>();
            List<Action> list = await ActionRepo.GetAllActions();
            list.ForEach(x => Actions.Add(ConvertToActionDto(x)));
            return Actions;
        }

        public async Task<Action> GetActionById(int id)
        {
            return await ActionRepo.GetActionById(id);
        }

        public async Task<Action> GetActionByLogin(int login)
        {
            Action? Action = (await ActionRepo.GetAllActions())
                                    .FirstOrDefault(x => x != null &&
                                                         !x.Login.IsNullOrEmpty() &&
                                                         x.Login.Equals(login));
            return Action;
        }

        public async Task<ActionDto> GetActionDtoById(string id)
        {
            return ConvertToActionDto(await ActionRepo.GetActionById(id));
        }

        public async Task<bool> SaveAction(Action ActionToSave)
        {
            return await ActionRepo.SaveAction(ActionToSave);
        }

        public async Task<bool> SaveAction(ActionDto ActionDtoToSave)
        {
            return await ActionRepo.SaveAction(ConvertToAction(ActionDtoToSave));
        }

        public async Task<bool> SaveAction(AddActionModel ActionDtoToAdd)
        {
            return await ActionRepo.SaveAction(ConvertToAction(ActionDtoToAdd));
        }*/
    }
}
