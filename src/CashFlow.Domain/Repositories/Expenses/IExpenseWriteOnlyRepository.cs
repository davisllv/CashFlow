using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;

public interface IExpenseWriteOnlyRepository
{
    Task Add(Expense expense);
    /// <param name="id"></param>
    /// <returns></returns>
    Task Delete(long id);
}
