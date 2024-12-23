using CashFlow.Communication.Request;

namespace CashFlow.Application.UseCase.Expenses.UpdateExpenseUseCase;
public interface IUpdateExpenseUseCase
{
    Task Execute(long id, RequestExpenseJson request);
}
