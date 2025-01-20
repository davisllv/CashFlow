using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;

public interface IExpenseUpdateOnlyRepository
{
    Task<Expense?> GetById(long id); // Tem esse getbyid e o do readonly, porém, esse aqui é específico porque eu não tenho o asnotracking, porque eu preciso salvar cache
    void Update(Expense expense);
}
