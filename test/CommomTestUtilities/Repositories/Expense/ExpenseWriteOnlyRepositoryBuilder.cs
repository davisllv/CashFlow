using CashFlow.Domain.Repositories.Expenses;
using Moq;

namespace CommomTestUtilities.Repositories.Expense;
public class ExpenseWriteOnlyRepositoryBuilder
{
    public static IExpenseWriteOnlyRepository Build()
    {
        var moq = new Mock<IExpenseWriteOnlyRepository>();

        return moq.Object;
    }
}
