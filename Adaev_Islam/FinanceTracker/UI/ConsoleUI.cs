namespace FinanceTracker.UI
{
    using Core.Services.Facades;
    using Core.Services.Commands;
    using Core.Services;
    using Domain.Models;
    using Domain.Visitors;
    using Domain.Factories;
    using Core.Repositories;
    using Core.Services.Import;

    /// <summary>
    /// Консольное меню
    /// </summary>
    public class ConsoleUi
    {
        private readonly AccountFacade _accounts;
        private readonly CategoryFacade _categories;
        private readonly OperationFacade _operations;
        private readonly AnalyticsFacade _analytics;
        private readonly BalanceRecalculator _recalculator;
        private readonly IEntityFactory _factory;
        private readonly IRepository<Operation> _operationRepo;
        private readonly IRepository<BankAccount> _accountRepo;

        public ConsoleUi(
            AccountFacade accounts,
            CategoryFacade categories,
            OperationFacade operations,
            AnalyticsFacade analytics,
            BalanceRecalculator recalculator,
            IEntityFactory factory,
            IRepository<Operation> operationRepo,
            IRepository<BankAccount> accountRepo)
        {
            _accounts = accounts;
            _categories = categories;
            _operations = operations;
            _analytics = analytics;
            _recalculator = recalculator;
            _factory = factory;
            _operationRepo = operationRepo;
            _accountRepo = accountRepo;
        }

        public void Run()
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine("=== Finance Tracker ===");
                Console.WriteLine("1. Manage Accounts");
                Console.WriteLine("2. Manage Categories");
                Console.WriteLine("3. Manage Operations");
                Console.WriteLine("4. Analytics");
                Console.WriteLine("5. Export Data");
                Console.WriteLine("6. Import Data");
                Console.WriteLine("7. Recalculate Balance");
                Console.WriteLine("0. Exit");
                Console.Write("\nChoice: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": ManageAccounts(); break;
                    case "2": ManageCategories(); break;
                    case "3": ManageOperations(); break;
                    case "4": ShowAnalytics(); break;
                    case "5": ExportData(); break;
                    case "6": ImportData(); break;
                    case "7": RecalculateBalance(); break;
                    case "0": return;
                }
            }
        }

        private void ManageAccounts()
        {
            Console.Clear();
            Console.WriteLine("=== Accounts ===");
            Console.WriteLine("1. Create Account");
            Console.WriteLine("2. List Accounts");
            Console.WriteLine("3. Delete Account");
            Console.Write("\nChoice: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("ID: ");
                    int id = int.Parse(Console.ReadLine());
                    Console.Write("Name: ");
                    string? name = Console.ReadLine();
                    Console.Write("Initial Balance: ");
                    decimal balance = decimal.Parse(Console.ReadLine());
                    _accounts.CreateAccount(id, name, balance);
                    Console.WriteLine("Account created!");
                    break;

                case "2":
                    foreach (BankAccount acc in _accounts.GetAllAccounts())
                    {
                        Console.WriteLine($"#{acc.Id} {acc.Name} - Balance: {acc.Balance:C}");
                    }

                    break;

                case "3":
                    Console.Write("Account ID: ");
                    int delId = int.Parse(Console.ReadLine());
                    _accounts.DeleteAccount(delId);
                    Console.WriteLine("Deleted!");
                    break;
            }

            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }

        private void ManageCategories()
        {
            Console.Clear();
            Console.WriteLine("=== Categories ===");
            Console.WriteLine("1. Create Category");
            Console.WriteLine("2. List Categories");
            Console.Write("\nChoice: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("ID: ");
                    int id = int.Parse(Console.ReadLine());
                    Console.Write("Name: ");
                    string? name = Console.ReadLine();
                    Console.Write("Type (0=Income, 1=Expense): ");
                    OperationType type = (OperationType)int.Parse(Console.ReadLine());
                    _categories.CreateCategory(id, name, type);
                    Console.WriteLine("Category created!");
                    break;

                case "2":
                    foreach (Category cat in _categories.GetAllCategories())
                    {
                        Console.WriteLine($"#{cat.Id} {cat.Name} ({cat.Type})");
                    }

                    break;
            }

            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }

        private void ManageOperations()
        {
            Console.Clear();
            Console.WriteLine("=== Operations ===");
            Console.WriteLine("1. Create Operation (with timing)");
            Console.WriteLine("2. List Operations");
            Console.WriteLine("3. Update Operation");
            Console.WriteLine("4. Delete Operation");
            Console.Write("\nChoice: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("ID: ");
                    int id = int.Parse(Console.ReadLine());
                    Console.Write("Type (0=Income, 1=Expense): ");
                    OperationType type = (OperationType)int.Parse(Console.ReadLine());
                    Console.Write("Account ID: ");
                    int accId = int.Parse(Console.ReadLine());
                    Console.Write("Amount: ");
                    decimal amount = decimal.Parse(Console.ReadLine());
                    Console.Write("Description: ");
                    string? desc = Console.ReadLine();
                    Console.Write("Category ID: ");
                    int catId = int.Parse(Console.ReadLine());

                    CreateOperationCommand command = new CreateOperationCommand(
                        _factory,
                        _operationRepo,
                        _accountRepo,
                        id, type, accId, amount, DateTime.Now, desc, catId
                    );

                    TimedCommandDecorator timedCommand = new TimedCommandDecorator(command, "CreateOperation");
                    timedCommand.Execute();

                    Console.WriteLine("Operation created!");
                    break;

                case "2":
                    foreach (Operation op in _operations.GetAllOperations())
                    {
                        Console.WriteLine($"#{op.Id} {op.Type} {op.Amount:C} - {op.Description}");
                    }

                    break;

                case "3":
                    Console.Write("Operation ID: ");
                    int updateId = int.Parse(Console.ReadLine());
                    Console.Write("New Amount: ");
                    decimal newAmount = decimal.Parse(Console.ReadLine());
                    Console.Write("New Description: ");
                    string? newDesc = Console.ReadLine();
                    _operations.UpdateOperation(updateId, newAmount, newDesc);
                    Console.WriteLine("Updated!");
                    break;

                case "4":
                    Console.Write("Operation ID: ");
                    int delId = int.Parse(Console.ReadLine());
                    _operations.DeleteOperation(delId);
                    Console.WriteLine("Deleted!");
                    break;
            }

            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }

        private void ShowAnalytics()
        {
            Console.Clear();
            Console.WriteLine("=== Analytics ===");

            DateTime from = DateTime.Now.AddMonths(-1);
            DateTime to = DateTime.Now;

            decimal income = _analytics.GetTotalIncome(from, to);
            decimal expense = _analytics.GetTotalExpense(from, to);
            decimal diff = _analytics.GetIncomeExpenseDifference(from, to);

            Console.WriteLine($"Period: {from:d} - {to:d}");
            Console.WriteLine($"Total Income: {income:C}");
            Console.WriteLine($"Total Expense: {expense:C}");
            Console.WriteLine($"Difference: {diff:C}");

            Console.WriteLine("\n--- Income by Category ---");
            Dictionary<string, decimal> incomeGroups = _analytics.GroupByCategory(from, to, OperationType.Income);
            foreach (KeyValuePair<string, decimal> group in incomeGroups)
            {
                Console.WriteLine($"{group.Key}: {group.Value:C}");
            }

            Console.WriteLine("\n--- Expense by Category ---");
            Dictionary<string, decimal> expenseGroups = _analytics.GroupByCategory(from, to, OperationType.Expense);
            foreach (KeyValuePair<string, decimal> group in expenseGroups)
            {
                Console.WriteLine($"{group.Key}: {group.Value:C}");
            }

            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }

        private void ExportData()
        {
            Console.Clear();
            Console.WriteLine("=== Export Data ===");
            Console.Write("Format (csv/json/yaml): ");
            string? format = Console.ReadLine();

            ExportVisitor visitor = new ExportVisitor(format);

            foreach (BankAccount acc in _accounts.GetAllAccounts())
            {
                visitor.Visit(acc);
            }

            foreach (Category cat in _categories.GetAllCategories())
            {
                visitor.Visit(cat);
            }

            foreach (Operation op in _operations.GetAllOperations())
            {
                visitor.Visit(op);
            }

            string result = visitor.GetResult();
            string filename = $"export_{DateTime.Now:yyyyMMdd_HHmmss}.{format}";
            File.WriteAllText(filename, result);

            Console.WriteLine($"Exported to {filename}");
            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }

        private void RecalculateBalance()
        {
            Console.Clear();
            Console.WriteLine("=== Recalculate Balance ===");
            Console.WriteLine("1. Recalculate specific account");
            Console.WriteLine("2. Recalculate all accounts");
            Console.Write("\nChoice: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Account ID: ");
                    int id = int.Parse(Console.ReadLine());
                    _recalculator.RecalculateAccount(id);
                    Console.WriteLine("Balance recalculated!");
                    break;

                case "2":
                    _recalculator.RecalculateAll();
                    Console.WriteLine("All balances recalculated!");
                    break;
            }

            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }

        private void ImportData()
        {
            Console.Clear();
            Console.WriteLine("=== Import Data ===");
            Console.Write("File path: ");
            string? filePath = Console.ReadLine();

            if (!File.Exists(filePath))
            {
                Console.WriteLine("File not found!");
                Console.ReadKey();
                return;
            }

            Console.Write("Format (csv/json/yaml): ");
            string? format = Console.ReadLine()?.ToLower();

            try
            {
                DataImporter importer = format switch
                {
                    "csv" => new CsvImporter(_operationRepo),
                    "json" => new JsonImporter(_operationRepo),
                    "yaml" => new YamlImporter(_operationRepo),
                    _ => throw new InvalidOperationException("Unknown format")
                };

                importer.Import(filePath);
                Console.WriteLine("Import successful!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Import failed: {ex.Message}");
            }

            Console.WriteLine("\nPress any key...");
            Console.ReadKey();
        }
    }
}