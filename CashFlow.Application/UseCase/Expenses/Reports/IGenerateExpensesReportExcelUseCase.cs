namespace CashFlow.Application.UseCase.Expenses.Reports;
public interface IGenerateExpensesReportExcelUseCase
{
    public Task<byte[]> Execute(DateOnly month);
}
