namespace FinanceTracker.Core.Services.Facades
{
    using Domain.Models;
    using Repositories;

    /// <summary>
    /// Паттерн: Facade
    /// Упрощает работу с аналитикой
    /// </summary>
    public class AnalyticsFacade
    {
        private readonly IRepository<Operation> _operationRepo;
        private readonly IRepository<Category> _categoryRepo;

        public AnalyticsFacade(
            IRepository<Operation> operationRepo,
            IRepository<Category> categoryRepo)
        {
            _operationRepo = operationRepo;
            _categoryRepo = categoryRepo;
        }

        public decimal GetIncomeExpenseDifference(DateTime from, DateTime to)
        {
            List<Operation> operations = _operationRepo.GetAll()
                .Where(o => o.Date >= from && o.Date <= to)
                .ToList();

            decimal income = operations
                .Where(o => o.Type == OperationType.Income)
                .Sum(o => o.Amount);

            decimal expense = operations
                .Where(o => o.Type == OperationType.Expense)
                .Sum(o => o.Amount);

            return income - expense;
        }

        public Dictionary<string, decimal> GroupByCategory(DateTime from, DateTime to, OperationType type)
        {
            List<Operation> operations = _operationRepo.GetAll()
                .Where(o => o.Date >= from && o.Date <= to && o.Type == type)
                .ToList();

            Dictionary<string, decimal> result = new Dictionary<string, decimal>();

            foreach (Operation operation in operations)
            {
                Category? category = _categoryRepo.GetById(operation.CategoryId);
                string categoryName = category?.Name ?? "Unknown";

                if (!result.ContainsKey(categoryName))
                {
                    result[categoryName] = 0;
                }

                result[categoryName] += operation.Amount;
            }

            return result;
        }

        public decimal GetTotalIncome(DateTime from, DateTime to)
        {
            return _operationRepo.GetAll()
                .Where(o => o.Date >= from && o.Date <= to && o.Type == OperationType.Income)
                .Sum(o => o.Amount);
        }

        public decimal GetTotalExpense(DateTime from, DateTime to)
        {
            return _operationRepo.GetAll()
                .Where(o => o.Date >= from && o.Date <= to && o.Type == OperationType.Expense)
                .Sum(o => o.Amount);
        }
    }
}