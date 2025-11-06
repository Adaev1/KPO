namespace FinanceTracker.Domain.Models
{
    /// <summary>
    /// Банковский счет с балансом
    /// </summary>
    public class BankAccount
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Balance { get; set; }

        public BankAccount(int id, string name, decimal balance)
        {
            Id = id;
            Name = name;
            Balance = balance;
        }

        public void UpdateBalance(decimal amount)
        {
            Balance += amount;
        }
    }
}