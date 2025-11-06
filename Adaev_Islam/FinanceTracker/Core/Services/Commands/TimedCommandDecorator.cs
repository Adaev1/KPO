namespace FinanceTracker.Core.Services.Commands
{
    using System.Diagnostics;

    /// <summary>
    /// Паттерн: Decorator
    /// Измеряет время выполнения команды
    /// </summary>
    public class TimedCommandDecorator : ICommand
    {
        private readonly ICommand _innerCommand;
        private readonly string _commandName;

        public TimedCommandDecorator(ICommand innerCommand, string commandName)
        {
            _innerCommand = innerCommand;
            _commandName = commandName;
        }

        public void Execute()
        {
            Stopwatch sw = Stopwatch.StartNew();
            _innerCommand.Execute();
            sw.Stop();
        
            Console.WriteLine($"[TIMING] {_commandName}: {sw.ElapsedMilliseconds}ms");
        }
    }
}