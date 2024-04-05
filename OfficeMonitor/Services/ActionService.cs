using CRUD.implementation;

namespace OfficeMonitor.Services
{
    public class ActionService
    {
        private ActionRepo actionRepo;
        public ActionService(ActionRepo _actionRepo)
        {
            actionRepo = _actionRepo;
        }
    }
}
