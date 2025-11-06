namespace FinanceTracker.Domain.Factories
{
    using Models;

    /// <summary>
    /// Фабрика для создания валидных доменных объектов
    /// </summary>
    public interface IEntityFactory
    {
        BankAccount CreateAccount(int id, string name, decimal initialBalance);
        Category CreateCategory(int id, string name, OperationType type);
        Operation CreateOperation(int id, OperationType type, int accountId, decimal amount, 
            DateTime date, string description, int categoryId);
    }
}