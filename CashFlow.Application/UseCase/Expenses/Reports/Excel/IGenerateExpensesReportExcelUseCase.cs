namespace CashFlow.Application.UseCase.Expenses.Reports.Excel;
public interface IGenerateExpensesReportExcelUseCase
{
    public Task<byte[]> Execute(DateOnly month);
}
