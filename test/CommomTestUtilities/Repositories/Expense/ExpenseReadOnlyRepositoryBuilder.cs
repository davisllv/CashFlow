using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CommomTestUtilities.Repositories.Expense;
public class ExpenseReadOnlyRepositoryBuilder
{
    private readonly Mock<IExpenseReadOnlyRepository> _expenseReadOnlyRepositoryMock;
    public ExpenseReadOnlyRepositoryBuilder()
    {
        _expenseReadOnlyRepositoryMock = new Mock<IExpenseReadOnlyRepository>();
    }

    public ExpenseReadOnlyRepositoryBuilder GetAll(CashFlow.Domain.Entities.User user, List<CashFlow.Domain.Entities.Expense> expenses)
    {
        _expenseReadOnlyRepositoryMock.Setup(setup => setup.GetAll(user)).ReturnsAsync(expenses);

        return this;
    }

    public ExpenseReadOnlyRepositoryBuilder GetById(CashFlow.Domain.Entities.User user, CashFlow.Domain.Entities.Expense expense)
    {
        _expenseReadOnlyRepositoryMock.Setup(setup => setup.GetById(user, expense.Id)).ReturnsAsync(expense);

        return this;
    }

    public IExpenseReadOnlyRepository Build() => _expenseReadOnlyRepositoryMock.Object;
}
