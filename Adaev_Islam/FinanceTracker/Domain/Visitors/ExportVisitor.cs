namespace FinanceTracker.Domain.Visitors
{
    using Models;
    using System.Text;

    /// <summary>
    /// Паттерн: Visitor
    /// Экспорт данных в различные форматы
    /// </summary>
    public class ExportVisitor : IEntityVisitor
    {
        private readonly StringBuilder _output = new();
        private readonly string _format;

        public ExportVisitor(string format)
        {
            _format = format;
        }

        public void Visit(BankAccount account)
        {
            switch (_format.ToLower())
            {
                case "csv":
                    _output.AppendLine($"{account.Id},{account.Name},{account.Balance}");
                    break;
                case "json":
                    _output.AppendLine($"{{\"id\":{account.Id},\"name\":\"{account.Name}\",\"balance\":{account.Balance}}}");
                    break;
                case "yaml":
                    _output.AppendLine($"- id: {account.Id}");
                    _output.AppendLine($"  name: {account.Name}");
                    _output.AppendLine($"  balance: {account.Balance}");
                    break;
            }
        }

        public void Visit(Category category)
        {
            switch (_format.ToLower())
            {
                case "csv":
                    _output.AppendLine($"{category.Id},{category.Name},{category.Type}");
                    break;
                case "json":
                    _output.AppendLine($"{{\"id\":{category.Id},\"name\":\"{category.Name}\",\"type\":\"{category.Type}\"}}");
                    break;
                case "yaml":
                    _output.AppendLine($"- id: {category.Id}");
                    _output.AppendLine($"  name: {category.Name}");
                    _output.AppendLine($"  type: {category.Type}");
                    break;
            }
        }

        public void Visit(Operation operation)
        {
            switch (_format.ToLower())
            {
                case "csv":
                    _output.AppendLine($"{operation.Id},{operation.Type},{operation.BankAccountId}," +
                                       $"{operation.Amount},{operation.Date:yyyy-MM-dd},{operation.Description},{operation.CategoryId}");
                    break;
                case "json":
                    _output.AppendLine($"{{\"id\":{operation.Id},\"type\":\"{operation.Type}\"," +
                                       $"\"accountId\":{operation.BankAccountId},\"amount\":{operation.Amount}," +
                                       $"\"date\":\"{operation.Date:yyyy-MM-dd}\",\"description\":\"{operation.Description}\"," +
                                       $"\"categoryId\":{operation.CategoryId}}}");
                    break;
                case "yaml":
                    _output.AppendLine($"- id: {operation.Id}");
                    _output.AppendLine($"  type: {operation.Type}");
                    _output.AppendLine($"  accountId: {operation.BankAccountId}");
                    _output.AppendLine($"  amount: {operation.Amount}");
                    _output.AppendLine($"  date: {operation.Date:yyyy-MM-dd}");
                    _output.AppendLine($"  description: {operation.Description}");
                    _output.AppendLine($"  categoryId: {operation.CategoryId}");
                    break;
            }
        }

        public string GetResult()
        {
            return _output.ToString();
        }
    }
}