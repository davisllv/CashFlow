using CashFlow.Communication.Request;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCase.Expenses.Register;
public interface IRegisterExpenseUseCase
{
    Task<ResponseRegisterExpenseJson> Execute(RequestRegisterExpenseJson request);
}
