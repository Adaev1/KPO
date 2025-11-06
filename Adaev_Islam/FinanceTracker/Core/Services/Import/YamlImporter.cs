namespace FinanceTracker.Core.Services.Import
{
    using Domain.Models;
    using Repositories;

    /// <summary>
    /// Паттерн: Template Method
    /// Импорт операций из YAML (упрощенный парсинг)
    /// </summary>
    public class YamlImporter : DataImporter
    {
        private readonly IRepository<Operation> _repository;

        public YamlImporter(IRepository<Operation> repository)
        {
            _repository = repository;
        }

        protected override List<Dictionary<string, string>> ParseContent(string content)
        {
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();
            string[] lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            Dictionary<string, string> current = null;

            foreach (string line in lines)
            {
                string trimmed = line.Trim();
                if (trimmed.StartsWith("- "))
                {
                    if (current != null)
                    {
                        result.Add(current);
                    }

                    current = new Dictionary<string, string>();
                
                    string[] parts = trimmed.Substring(2).Split(':', 2);
                    if (parts.Length == 2)
                    {
                        current[parts[0].Trim()] = parts[1].Trim();
                    }
                }
                else if (current != null && trimmed.Contains(':'))
                {
                    string[] parts = trimmed.Split(':', 2);
                    if (parts.Length == 2)
                    {
                        current[parts[0].Trim()] = parts[1].Trim();
                    }
                }
            }

            if (current != null)
            {
                result.Add(current);
            }

            return result;
        }

        protected override void SaveData(List<Dictionary<string, string>> data)
        {
            foreach (Dictionary<string, string> row in data)
            {
                if (!row.ContainsKey("id") || !row.ContainsKey("amount"))
                {
                    continue;
                }

                Operation operation = new Operation(
                    int.Parse(row["id"]),
                    Enum.Parse<OperationType>(row["type"]),
                    int.Parse(row["accountId"]),
                    decimal.Parse(row["amount"]),
                    DateTime.Parse(row["date"]),
                    row.GetValueOrDefault("description", ""),
                    int.Parse(row["categoryId"])
                );

                _repository.Add(operation);
            }
        }
    }
}