using CoreShop.CORE.Entity;

namespace CoreShop.CORE.Service
{
    public interface ICoreService<T> where T : CoreEntity
    {
        /// <summary>Persists the entity, assigns its ID and returns it.</summary>
        T Create(T entity);
        bool Update(T entity);
        bool Delete(T entity);
        T? GetById(int id);
        List<T> GetAll();
    }
}
