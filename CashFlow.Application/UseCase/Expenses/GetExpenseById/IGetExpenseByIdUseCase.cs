using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCase.Expenses.GetExpenseById;
public interface IGetExpenseByIdUseCase
{
    public Task<ResponseExpenseJson> Execute(long id);
}
