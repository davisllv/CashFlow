using CashFlow.Communication.Responses.Expenses;

namespace CashFlow.Application.UseCase.Expenses.GetExpenseById;
public interface IGetExpenseByIdUseCase
{
    // --- Verified Commit
    public Task<ResponseExpenseJson> Execute(long id);
}
