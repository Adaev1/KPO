namespace FinanceTracker.Core.Repositories
{
    /// <summary>
    /// Простое хранилище в памяти
    /// </summary>
    public class InMemoryRepository<T> : IRepository<T> where T : class
    {
        protected readonly Dictionary<int, T> Storage = new();
        protected readonly Func<T, int> IdGetter;

        public InMemoryRepository(Func<T, int> idGetter)
        {
            IdGetter = idGetter;
        }

        public virtual void Add(T entity)
        {
            int id = IdGetter(entity);
            Storage[id] = entity;
        }

        public virtual T GetById(int id)
        {
            return Storage.TryGetValue(id, out T? entity) ? entity : null;
        }

        public virtual IReadOnlyList<T> GetAll()
        {
            return Storage.Values.ToList();
        }

        public virtual void Update(T entity)
        {
            int id = IdGetter(entity);
            if (Storage.ContainsKey(id))
            {
                Storage[id] = entity;
            }
        }

        public virtual void Delete(int id)
        {
            Storage.Remove(id);
        }
    }
}