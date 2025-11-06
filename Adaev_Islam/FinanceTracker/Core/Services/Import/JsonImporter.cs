namespace FinanceTracker.Core.Services.Import
{
    using Domain.Models;
    using Repositories;
    using System.Text.Json;

    /// <summary>
    /// Паттерн: Template Method
    /// Импорт операций из JSON
    /// </summary>
    public class JsonImporter : DataImporter
    {
        private readonly IRepository<Operation> _repository;

        public JsonImporter(IRepository<Operation> repository)
        {
            _repository = repository;
        }

        protected override List<Dictionary<string, string>> ParseContent(string content)
        {
            List<Dictionary<string, JsonElement>>? jsonArray = JsonSerializer.Deserialize<List<Dictionary<string, JsonElement>>>(content);
            List<Dictionary<string, string>> result = new List<Dictionary<string, string>>();

            if (jsonArray == null)
            {
                return result;
            }

            foreach (Dictionary<string, JsonElement> item in jsonArray)
            {
                Dictionary<string, string> row = new Dictionary<string, string>();
                foreach (KeyValuePair<string, JsonElement> kvp in item)
                {
                    row[kvp.Key] = kvp.Value.ToString();
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