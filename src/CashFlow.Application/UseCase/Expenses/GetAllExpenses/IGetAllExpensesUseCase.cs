using CashFlow.Communication.Responses.Expenses;

namespace CashFlow.Application.UseCase.Expenses.GetAllExpenses;
public interface IGetAllExpensesUseCase
{
    Task<ResponseExpensesJson> Execute();
}
