namespace FinanceTracker.Core.Services.Commands
{
    using Domain.Models;
    using Repositories;

    /// <summary>
    /// Паттерн: Command
    /// Команда удаления операции
    /// </summary>
    public class DeleteOperationCommand : ICommand
    {
        private readonly IRepository<Operation> _operationRepo;
        private readonly IRepository<BankAccount> _accountRepo;
        private readonly int _operationId;

        public DeleteOperationCommand(
            IRepository<Operation> operationRepo,
            IRepository<BankAccount> accountRepo,
            int operationId)
        {
            _operationRepo = operationRepo;
            _accountRepo = accountRepo;
            _operationId = operationId;
        }

        public void Execute()
        {
            Operation? operation = _operationRepo.GetById(_operationId);
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

            _operationRepo.Delete(_operationId);
        }
    }
}