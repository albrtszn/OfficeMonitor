using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.interfaces
{
    public interface IntRepoInterface<T>
    {
        Task<List<T>> GetAll();
        Task<T?> GetById(int id);
        Task<T?> GetTrackById(int id);
        Task<bool> Save(T entityToSave);
        Task<bool> DeleteById(int id);
    }
}
