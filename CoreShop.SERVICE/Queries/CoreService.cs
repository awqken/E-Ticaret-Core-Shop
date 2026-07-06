using CoreShop.CORE.Entity;
using CoreShop.CORE.Service;
using CoreShop.SERVICE.Data;

namespace CoreShop.SERVICE.Queries
{
    public class CoreService<T> : ICoreService<T> where T : CoreEntity
    {
        private static readonly object _lock = new();

        private List<T> Store => InMemoryStore.GetList<T>();

        public List<T> GetAll()
        {
            lock (_lock) return Store.ToList();
        }

        public T? GetById(int id)
        {
            lock (_lock) return Store.FirstOrDefault(x => x.ID == id);
        }

        public T Create(T entity)
        {
            lock (_lock)
            {
                entity.ID = Store.Count == 0 ? 1 : Store.Max(x => x.ID) + 1;
                Store.Add(entity);
                return entity;
            }
        }

        public bool Update(T entity)
        {
            lock (_lock)
            {
                var idx = Store.FindIndex(x => x.ID == entity.ID);
                if (idx < 0) return false;
                Store[idx] = entity;
                return true;
            }
        }

        public bool Delete(T entity)
        {
            lock (_lock)
            {
                var item = Store.FirstOrDefault(x => x.ID == entity.ID);
                if (item == null) return false;
                return Store.Remove(item);
            }
        }
    }
}
