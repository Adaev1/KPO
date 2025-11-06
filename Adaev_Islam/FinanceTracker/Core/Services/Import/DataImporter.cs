namespace FinanceTracker.Core.Services.Import
{
    /// <summary>
    /// Паттерн: Template Method
    /// Базовый класс для импорта данных из файлов
    /// </summary>
    public abstract class DataImporter
    {
        public void Import(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException($"File not found: {filePath}");
            }

            string content = File.ReadAllText(filePath);
            List<Dictionary<string, string>> data = ParseContent(content);
            ValidateData(data);
            SaveData(data);
        }

        protected abstract List<Dictionary<string, string>> ParseContent(string content);

        protected virtual void ValidateData(List<Dictionary<string, string>> data)
        {
            if (data == null || data.Count == 0)
            {
                throw new InvalidOperationException("No data to import");
            }
        }

        protected abstract void SaveData(List<Dictionary<string, string>> data);
    }
}