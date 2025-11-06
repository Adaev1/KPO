namespace FinanceTracker.Domain.Visitors
{
    using Models;

    /// <summary>
    /// Паттерн: Visitor
    /// Позволяет добавлять новые операции над объектами без изменения их классов
    /// </summary>
    public interface IEntityVisitor
    {
        void Visit(BankAccount account);
        void Visit(Category category);
        void Visit(Operation operation);
    }
}