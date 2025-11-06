namespace FinanceTracker.Core.Services.Facades
{
    using Domain.Factories;
    using Domain.Models;
    using Repositories;

    /// <summary>
    /// Паттерн: Facade
    /// Упрощает работу с категориями
    /// </summary>
    public class CategoryFacade
    {
        private readonly IEntityFactory _factory;
        private readonly IRepository<Category> _repository;

        public CategoryFacade(IEntityFactory factory, IRepository<Category> repository)
        {
            _factory = factory;
            _repository = repository;
        }

        public Category CreateCategory(int id, string name, OperationType type)
        {
            Category category = _factory.CreateCategory(id, name, type);
            _repository.Add(category);
            return category;
        }

        public Category GetCategory(int id)
        {
            return _repository.GetById(id);
        }

        public IReadOnlyList<Category> GetAllCategories()
        {
            return _repository.GetAll();
        }

        public IReadOnlyList<Category> GetByType(OperationType type)
        {
            return _repository.GetAll()
                .Where(c => c.Type == type)
                .ToList();
        }

        public void UpdateCategory(Category category)
        {
            _repository.Update(category);
        }

        public void DeleteCategory(int id)
        {
            _repository.Delete(id);
        }
    }
}