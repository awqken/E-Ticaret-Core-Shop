using CoreShop.CORE.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreShop.CORE.Service
{
    public interface ICoreService<T> where T : CoreEntity
    {
        bool Create(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        T GetById(int id);
        List<T> GetAll();
    }
}
