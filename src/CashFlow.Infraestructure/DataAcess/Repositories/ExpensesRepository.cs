using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAccess.Respositories;

internal class ExpensesRepository : IExpenseReadOnlyRepository, IExpenseWriteOnlyRepository, IExpenseUpdateOnlyRepository
{
    private readonly CashFlowDbContext _dbContext;
    public ExpensesRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task Add(Expense expense)
    {
        await _dbContext.Expenses.AddAsync(expense);
        // Não colocar o commit no repositório e sim na regra de negócio.
    }

    public async Task Delete(long id)
    {
        var response = await _dbContext.Expenses.FirstAsync(expense => expense.Id == id);

        _dbContext.Expenses.Remove(response);
    }

    public async Task<List<Expense>> GetAll(User user)
    {
        // AsNoTracking - Não é preciso salvar cache de nada, visto que não vai ser feito nenhuma alteração. Torna as consultas mais rápidas. Quando for feito uma alteração no banco não deve ser feito assim.
        return await _dbContext.Expenses.AsNoTracking().Where(expense => expense.UserId == user.Id).ToListAsync();
    }

    async Task<Expense?> IExpenseReadOnlyRepository.GetById(User user, long id)
    {
        return await GetFullExpense()
            .AsNoTracking()
            .FirstOrDefaultAsync(expense => expense.Id == id && user.Id == expense.UserId);
    }

    // Essa é uma forma de diferenciar quais os métodos estão sendo chamados de acordo com a interface
    async Task<Expense?> IExpenseUpdateOnlyRepository.GetById(User user, long id)
    {
        return await GetFullExpense()
            .FirstOrDefaultAsync(expense => expense.Id == id && expense.UserId == user.Id);
    }

    public void Update(Expense expense)
    {
        _dbContext.Expenses.Update(expense); // Update é void porque ele não é async
    }

    public async Task<List<Expense>> FilterByMonth(User user, DateOnly date)
    {
        // Filtros são feitos dentro do repositório.
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;

        var finalDate = new DateTime(year: date.Year, month: date.Month, day: DateTime.DaysInMonth(date.Year, date.Month), hour: 23, minute: 59, second: 59).Date;


        return await _dbContext
            .Expenses
            .AsNoTracking()
            .Where(expense => expense.Date >= startDate && expense.Date <= finalDate && expense.UserId == user.Id)
            .OrderBy(expense => expense.Date)
            .ThenBy(expense => expense.Title) // Forma de ordenar caso os valores estejam iguai
            .ToListAsync();
    }

    private Microsoft.EntityFrameworkCore.Query.IIncludableQueryable<Expense, ICollection<Tag>> GetFullExpense()
    {
        return _dbContext.Expenses
            .Include(expense => expense.Tags);
            //.ThenInclude(tag => tag.Expense) - Forma de dar mais includes
            //.Include(Expense => Expense.Tags).ThenInclude(Tag => Tag.user) - Para dar mais includes ainda.
        
    }
}