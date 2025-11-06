namespace FinanceTracker.Core.Services.Commands
{
    /// <summary>
    /// Паттерн: Command
    /// Инкапсуляция пользовательских действий
    /// </summary>
    public interface ICommand
    {
        void Execute();
    }
}