namespace FinanceTracker.Domain.Factories
{
    using Models;
    using Validation;

    /// <summary>
    /// Паттерн: Factory
    /// Централизованное создание объектов с валидацией
    /// </summary>
    public class EntityFactory : IEntityFactory
    {
        public BankAccount CreateAccount(int id, string name, decimal initialBalance)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException("Account name cannot be empty");
            }

            return new BankAccount(id, name, initialBalance);
        }

        public Category CreateCategory(int id, string name, OperationType type)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ValidationException("Category name cannot be empty");
            }

            return new Category(id, name, type);
        }

        public Operation CreateOperation(int id, OperationType type, int accountId, 
            decimal amount, DateTime date, string description, int categoryId)
        {
            if (amount <= 0)
            {
                throw new ValidationException("Amount must be positive");
            }

            if (accountId <= 0)
            {
                throw new ValidationException("Invalid account ID");
            }

            if (categoryId <= 0)
            {
                throw new ValidationException("Invalid category ID");
            }

            return new Operation(id, type, accountId, amount, date, description, categoryId);
        }
    }
}