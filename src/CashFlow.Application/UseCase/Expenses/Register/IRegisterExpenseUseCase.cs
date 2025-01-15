using CashFlow.Communication.Request;
using CashFlow.Communication.Responses.Expenses;

namespace CashFlow.Application.UseCase.Expenses.Register;
public interface IRegisterExpenseUseCase
{
    Task<ResponseRegisterExpenseJson> Execute(RequestExpenseJson request);
}
