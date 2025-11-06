namespace FinanceTracker.Core.Services.Import
{
    using Domain.Models;
    using Repositories;

    /// <summary>
    /// Паттерн: Template Method
    /// Импорт операций из CSV
    /// </summary>
    public class CsvImporter : DataImporter
    {
        private readonly IRepository<Operation> _repository;

        public CsvImporter(IRepository<Operation> repository)
        {
            _repository = repository;
        }

        protected override List<Dictionary<string, string>> ParseContent(string content)
        {
            string[] lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

            if (lines.Length < 2)
            {
                return result;
            }

            string[] headers = lines[0].Split(',');

            for (int i = 1; i < lines.Length; i++)
            {
                string[] values = lines[i].Split(',');
                Dictionary<string, string> row = new Dictionary<string, string>();

                for (int j = 0; j < headers.Length && j < values.Length; j++)
                {
                    row[headers[j].Trim()] = values[j].Trim();
                }

                result.Add(row);
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