namespace CashFlow.Application.UseCase.Expenses.DeleteExpenseUseCase;
public interface IDeleteExpenseUseCase
{
    public Task Execute(long id);
}
