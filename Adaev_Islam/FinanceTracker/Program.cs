using Microsoft.Extensions.DependencyInjection;
using FinanceTracker.Domain.Factories;
using FinanceTracker.Domain.Models;
using FinanceTracker.Core.Repositories;
using FinanceTracker.Core.Services;
using FinanceTracker.Core.Services.Facades;
using FinanceTracker.UI;

ServiceProvider services = new ServiceCollection()
    .AddSingleton<IEntityFactory, EntityFactory>()
    .AddSingleton<IRepository<BankAccount>>(sp => 
        new CachedRepository<BankAccount>(
            new InMemoryRepository<BankAccount>(a => a.Id), 
            a => a.Id))
    .AddSingleton<IRepository<Category>>(sp => 
        new CachedRepository<Category>(
            new InMemoryRepository<Category>(c => c.Id), 
            c => c.Id))
    .AddSingleton<IRepository<Operation>>(sp => 
        new CachedRepository<Operation>(
            new InMemoryRepository<Operation>(o => o.Id), 
            o => o.Id))
    .AddSingleton<AccountFacade>()
    .AddSingleton<CategoryFacade>()
    .AddSingleton<OperationFacade>()
    .AddSingleton<AnalyticsFacade>()
    .AddSingleton<BalanceRecalculator>()
    .AddSingleton<ConsoleUi>()
    .BuildServiceProvider();

ConsoleUi ui = services.GetRequiredService<ConsoleUi>();

SeedData(services);

ui.Run();

static void SeedData(IServiceProvider services)
{
    AccountFacade accounts = services.GetRequiredService<AccountFacade>();
    CategoryFacade categories = services.GetRequiredService<CategoryFacade>();
    OperationFacade operations = services.GetRequiredService<OperationFacade>();

    accounts.CreateAccount(1, "Main Account", 10000);
    accounts.CreateAccount(2, "Savings", 50000);

    categories.CreateCategory(1, "Salary", OperationType.Income);
    categories.CreateCategory(2, "Cashback", OperationType.Income);
    categories.CreateCategory(3, "Groceries", OperationType.Expense);
    categories.CreateCategory(4, "Transport", OperationType.Expense);

    operations.CreateOperation(1, OperationType.Income, 1, 50000, 
        DateTime.Now.AddDays(-10), "Monthly salary", 1);
    operations.CreateOperation(2, OperationType.Expense, 1, 5000, 
        DateTime.Now.AddDays(-8), "Food shopping", 3);
    operations.CreateOperation(3, OperationType.Expense, 1, 1200, 
        DateTime.Now.AddDays(-5), "Metro card", 4);
    operations.CreateOperation(4, OperationType.Income, 1, 500, 
        DateTime.Now.AddDays(-3), "Cashback from bank", 2);
}
