using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
namespace CashFlow.Infrastructure.DataAccess.Respositories;

internal class ExpensesRepository : IExpensesRepository
{
    private readonly CashFlowDbContext _dbContext;
    public ExpensesRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Add(Expense expense)
    {
        _dbContext.Expenses.Add(expense);
        // Não colocar o commit no repositório e sim na regra de negócio.
    }
}
