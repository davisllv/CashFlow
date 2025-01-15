using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCase.Expenses.GetAllExpenses;
public interface IGetAllExpensesUseCase
{
    Task<ResponseExpensesJson> Execute();
}
