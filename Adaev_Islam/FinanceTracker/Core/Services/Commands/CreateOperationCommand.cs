namespace FinanceTracker.Core.Services.Commands
{
    using Domain.Factories;
    using Domain.Models;
    using Repositories;

    /// <summary>
    /// Паттерн: Command
    /// Команда создания операции
    /// </summary>
    public class CreateOperationCommand : ICommand
    {
        private readonly IEntityFactory _factory;
        private readonly IRepository<Operation> _operationRepo;
        private readonly IRepository<BankAccount> _accountRepo;
        private readonly int _id;
        private readonly OperationType _type;
        private readonly int _accountId;
        private readonly decimal _amount;
        private readonly DateTime _date;
        private readonly string _description;
        private readonly int _categoryId;

        public CreateOperationCommand(
            IEntityFactory factory,
            IRepository<Operation> operationRepo,
            IRepository<BankAccount> accountRepo,
            int id, OperationType type, int accountId, decimal amount,
            DateTime date, string description, int categoryId)
        {
            _factory = factory;
            _operationRepo = operationRepo;
            _accountRepo = accountRepo;
            _id = id;
            _type = type;
            _accountId = accountId;
            _amount = amount;
            _date = date;
            _description = description;
            _categoryId = categoryId;
        }

        public void Execute()
        {
            Operation operation = _factory.CreateOperation(_id, _type, _accountId, 
                _amount, _date, _description, _categoryId);
        
            _operationRepo.Add(operation);

            BankAccount? account = _accountRepo.GetById(_accountId);
            if (account != null)
            {
                decimal changeAmount = _type == OperationType.Income ? _amount : -_amount;
                account.UpdateBalance(changeAmount);
                _accountRepo.Update(account);
            }
        }
    }
}