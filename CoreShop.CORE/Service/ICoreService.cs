using CoreShop.CORE.Entity;

namespace CoreShop.CORE.Service
{
    public interface ICoreService<T> where T : CoreEntity
    {
        bool Create(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        T? GetById(int id);
        List<T> GetAll();
    }
}
