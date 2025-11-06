namespace FinanceTracker.Core.Services
{
    using Domain.Models;
    using Repositories;

    /// <summary>
    /// Сервис пересчета баланса счетов
    /// </summary>
    public class BalanceRecalculator
    {
        private readonly IRepository<BankAccount> _accountRepo;
        private readonly IRepository<Operation> _operationRepo;

        public BalanceRecalculator(
            IRepository<BankAccount> accountRepo,
            IRepository<Operation> operationRepo)
        {
            _accountRepo = accountRepo;
            _operationRepo = operationRepo;
        }

        public void RecalculateAccount(int accountId)
        {
            BankAccount? account = _accountRepo.GetById(accountId);
            if (account == null)
            {
                return;
            }

            List<Operation> operations = _operationRepo.GetAll()
                .Where(o => o.BankAccountId == accountId)
                .ToList();

            decimal balance = 0;
            foreach (Operation operation in operations)
            {
                balance += operation.Type == OperationType.Income 
                    ? operation.Amount 
                    : -operation.Amount;
            }

            account.Balance = balance;
            _accountRepo.Update(account);
        }

        public void RecalculateAll()
        {
            IReadOnlyList<BankAccount> accounts = _accountRepo.GetAll();
            foreach (BankAccount account in accounts)
            {
                RecalculateAccount(account.Id);
            }
        }
    }
}