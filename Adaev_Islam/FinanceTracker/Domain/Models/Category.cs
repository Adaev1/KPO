namespace FinanceTracker.Domain.Models
{
    /// <summary>
    /// Категория операции (доход или расход)
    /// </summary>
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public OperationType Type { get; set; }

        public Category(int id, string name, OperationType type)
        {
            Id = id;
            Name = name;
            Type = type;
        }
    }
}