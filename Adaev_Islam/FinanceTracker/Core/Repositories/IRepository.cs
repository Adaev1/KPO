namespace FinanceTracker.Core.Repositories
{
    /// <summary>
    /// Базовый репозиторий для работы с данными
    /// </summary>
    public interface IRepository<T> where T : class
    {
        void Add(T entity);
        T GetById(int id);
        IReadOnlyList<T> GetAll();
        void Update(T entity);
        void Delete(int id);
    }
}