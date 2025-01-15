namespace CashFlow.Application.UseCase.Expenses.Reports.Pdf;
public interface IGenerateExpensesReportPdfUseCase
{
    public Task<byte[]> Execute(DateOnly month);
}
