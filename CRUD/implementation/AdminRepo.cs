using CRUD.interfaces;
using OfficeMonitor.DataBase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CRUD.implementation
{
    public class AdminRepo : IntRepoInterface<Admin>
    {
        public Task<bool> DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Admin>> GetAll()
        {
            throw new NotImplementedException();
        }

        public Task<Admin> GetById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Admin> GetTrackById(int id)
        {
            throw new NotImplementedException();
        }

        public Task<bool> Save(Admin entityToSave)
        {
            throw new NotImplementedException();
        }
    }
}
