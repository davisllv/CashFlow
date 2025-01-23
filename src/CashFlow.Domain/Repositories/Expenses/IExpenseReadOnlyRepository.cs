using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;
public interface IExpenseReadOnlyRepository
{
    Task<List<Expense>> GetAll(User user);
    Task<Expense?> GetById(User user, long id); // Para definir que o valor pode retornar nulo
    Task<List<Expense>> FilterByMonth(User user, DateOnly date);
}
