using CashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Application.UseCase.Expenses.Reports.Pdf;

public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private const string CURRENCY_SYMBOL = "$";
    private readonly IExpenseReadOnlyRepository _repository;

    public GenerateExpensesReportPdfUseCase(IExpenseReadOnlyRepository repository)
    {
        _repository = repository;
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var expense = await _repository.FilterByMonth(month);

        if (expense.Count == 0)
            return [];

        return [];
    }
}
