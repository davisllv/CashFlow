using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;
using System;

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

    public async Task<bool> Delete(long id)
    {
        var response = await _dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);

        if(response is null)
        {
            return false;
        }

        _dbContext.Expenses.Remove(response);

        return true;
    }

    public async Task<List<Expense>> GetAll()
    {
        // AsNoTracking - Não é preciso salvar cache de nada, visto que não vai ser feito nenhuma alteração. Torna as consultas mais rápidas. Quando for feito uma alteração no banco não deve ser feito assim.
        return await _dbContext.Expenses.AsNoTracking().ToListAsync();
    }

    async Task<Expense?> IExpenseReadOnlyRepository.GetById(long id)
    {
        return await _dbContext.Expenses.AsNoTracking().FirstOrDefaultAsync(expense => expense.Id == id);
    }

    // Essa é uma forma de diferenciar quais os métodos estão sendo chamados de acordo com a interface
    async Task<Expense?> IExpenseUpdateOnlyRepository.GetById(long id)
    {
        return await _dbContext.Expenses.FirstOrDefaultAsync(expense => expense.Id == id);
    }

    public void Update(Expense expense)
    {
        _dbContext.Expenses.Update(expense); // Update é void porque ele não é async
    }

    public async Task<List<Expense>> FilterByMonth(DateOnly date)
    {
        // Filtros são feitos dentro do repositório.
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;

        var finalDate = new DateTime(year: date.Year, month: date.Month, day: DateTime.DaysInMonth(date.Year, date.Month), hour: 23, minute: 59, second: 59).Date;


        return await _dbContext
            .Expenses
            .AsNoTracking()
            .Where(expense => expense.Date >= startDate && expense.Date <= finalDate)
            .OrderBy(expense => expense.Date)
            .ThenBy(expense => expense.Title) // Forma de ordenar caso os valores estejam iguai
            .ToListAsync();
    }
}
