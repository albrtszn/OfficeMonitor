using CRUD.implementation;

namespace OfficeMonitor.Services
{
    public class AdminService
    {
        private AdminRepo AdminRepo;
        public AdminService(AdminRepo _AdminRepo)
        {
            AdminRepo = _AdminRepo;
        }
    }
}
