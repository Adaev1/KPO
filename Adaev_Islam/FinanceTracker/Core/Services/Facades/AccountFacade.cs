namespace FinanceTracker.Core.Services.Facades
{
    using Domain.Factories;
    using Domain.Models;
    using Repositories;

    /// <summary>
    /// Паттерн: Facade
    /// Упрощает работу со счетами
    /// </summary>
    public class AccountFacade
    {
        private readonly IEntityFactory _factory;
        private readonly IRepository<BankAccount> _repository;

        public AccountFacade(IEntityFactory factory, IRepository<BankAccount> repository)
        {
            _factory = factory;
            _repository = repository;
        }

        public BankAccount CreateAccount(int id, string name, decimal initialBalance)
        {
            BankAccount account = _factory.CreateAccount(id, name, initialBalance);
            _repository.Add(account);
            return account;
        }

        public BankAccount GetAccount(int id)
        {
            return _repository.GetById(id);
        }

        public IReadOnlyList<BankAccount> GetAllAccounts()
        {
            return _repository.GetAll();
        }

        public void UpdateAccount(BankAccount account)
        {
            _repository.Update(account);
        }

        public void DeleteAccount(int id)
        {
            _repository.Delete(id);
        }
    }
}