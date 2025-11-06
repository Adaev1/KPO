namespace FinanceTracker.Core.Services.Facades
{
    using Domain.Factories;
    using Domain.Models;
    using Repositories;

    /// <summary>
    /// Паттерн: Facade
    /// Упрощает работу с операциями
    /// </summary>
    public class OperationFacade
    {
        private readonly IEntityFactory _factory;
        private readonly IRepository<Operation> _operationRepo;
        private readonly IRepository<BankAccount> _accountRepo;

        public OperationFacade(
            IEntityFactory factory,
            IRepository<Operation> operationRepo,
            IRepository<BankAccount> accountRepo)
        {
            _factory = factory;
            _operationRepo = operationRepo;
            _accountRepo = accountRepo;
        }

        public Operation CreateOperation(int id, OperationType type, int accountId,
            decimal amount, DateTime date, string description, int categoryId)
        {
            Operation operation = _factory.CreateOperation(id, type, accountId, amount, 
                date, description, categoryId);
        
            _operationRepo.Add(operation);

            BankAccount? account = _accountRepo.GetById(accountId);
            if (account != null)
            {
                decimal changeAmount = type == OperationType.Income ? amount : -amount;
                account.UpdateBalance(changeAmount);
                _accountRepo.Update(account);
            }

            return operation;
        }

        public Operation GetOperation(int id)
        {
            return _operationRepo.GetById(id);
        }

        public IReadOnlyList<Operation> GetAllOperations()
        {
            return _operationRepo.GetAll();
        }

        public IReadOnlyList<Operation> GetByAccount(int accountId)
        {
            return _operationRepo.GetAll()
                .Where(o => o.BankAccountId == accountId)
                .ToList();
        }

        public IReadOnlyList<Operation> GetByPeriod(DateTime from, DateTime to)
        {
            return _operationRepo.GetAll()
                .Where(o => o.Date >= from && o.Date <= to)
                .ToList();
        }

        public void DeleteOperation(int id)
        {
            Operation? operation = _operationRepo.GetById(id);
            if (operation == null)
            {
                return;
            }

            BankAccount? account = _accountRepo.GetById(operation.BankAccountId);
            if (account != null)
            {
                decimal changeAmount = operation.Type == OperationType.Income 
                    ? -operation.Amount 
                    : operation.Amount;
                account.UpdateBalance(changeAmount);
                _accountRepo.Update(account);
            }

            _operationRepo.Delete(id);
        }
    
        public void UpdateOperation(int id, decimal newAmount, string newDescription)
        {
            Operation? operation = _operationRepo.GetById(id);
            if (operation == null)
            {
                return;
            }

            BankAccount? account = _accountRepo.GetById(operation.BankAccountId);
            if (account != null)
            {
                decimal oldChange = operation.Type == OperationType.Income 
                    ? -operation.Amount 
                    : operation.Amount;
                account.UpdateBalance(oldChange);
            }

            operation.Amount = newAmount;
            operation.Description = newDescription;
            _operationRepo.Update(operation);

            if (account != null)
            {
                decimal newChange = operation.Type == OperationType.Income 
                    ? newAmount 
                    : -newAmount;
                account.UpdateBalance(newChange);
                _accountRepo.Update(account);
            }
        }
    }
}