namespace FinanceTracker.Core.Repositories
{
    /// <summary>
    /// Паттерн: Proxy
    /// Добавляет кэширование к базовому репозиторию
    /// </summary>
    public class CachedRepository<T> : IRepository<T> where T : class
    {
        private readonly IRepository<T> _innerRepository;
        private readonly Dictionary<int, T> _cache = new();
        private readonly Func<T, int> _idGetter;
        private bool _cacheLoaded;

        public CachedRepository(IRepository<T> innerRepository, Func<T, int> idGetter)
        {
            _innerRepository = innerRepository;
            _idGetter = idGetter;
        }

        private void EnsureCacheLoaded()
        {
            if (_cacheLoaded)
            {
                return;
            }

            foreach (T item in _innerRepository.GetAll())
            {
                _cache[_idGetter(item)] = item;
            }
            _cacheLoaded = true;
        }

        public void Add(T entity)
        {
            _innerRepository.Add(entity);
            _cache[_idGetter(entity)] = entity;
        }

        public T GetById(int id)
        {
            EnsureCacheLoaded();
            return _cache.TryGetValue(id, out T? entity) ? entity : null;
        }

        public IReadOnlyList<T> GetAll()
        {
            EnsureCacheLoaded();
            return _cache.Values.ToList();
        }

        public void Update(T entity)
        {
            _innerRepository.Update(entity);
            _cache[_idGetter(entity)] = entity;
        }

        public void Delete(int id)
        {
            _innerRepository.Delete(id);
            _cache.Remove(id);
        }
    }
}