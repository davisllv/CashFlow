using CashFlow.Application.UseCase.Expenses.Reports.Pdf.Fonts;
using CashFlow.Domain.Reports;
using CashFlow.Domain.Repositories.Expenses;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using PdfSharp.Fonts;

namespace CashFlow.Application.UseCase.Expenses.Reports.Pdf;

public class GenerateExpensesReportPdfUseCase : IGenerateExpensesReportPdfUseCase
{
    private const string CURRENCY_SYMBOL = "$";
    private readonly IExpenseReadOnlyRepository _repository;

    public GenerateExpensesReportPdfUseCase(IExpenseReadOnlyRepository repository)
    {
        _repository = repository;
        // Forma de resolver o tipo de font do pdf, ao colocar dentro do useCase ele faz com que as fontes são utilizados apenas nesse relatório
        GlobalFontSettings.FontResolver = new ExpensesReportFontResolver();
    }

    public async Task<byte[]> Execute(DateOnly month)
    {
        var expense = await _repository.FilterByMonth(month);

        if (expense.Count == 0)
            return [];

        Document document = CreateDocument(month);
        Section page = CreatePage(document);

        Paragraph paragraph = page.AddParagraph();
        string title = string.Format(ResourceReportGenerationMessages.TOTAL_SPENT_IN, month.ToString("Y"));

        paragraph.AddFormattedText(title, new Font { Name = FontHelper.RALEWAY_REGULAR, Size = 15});

        paragraph.AddLineBreak(); // Adicionar uma quebra de linha
        paragraph.AddFormattedText($"{expense.Sum(ex => ex.Amount)} {CURRENCY_SYMBOL}", new Font { Name = FontHelper.WORKSANS_BLACK, Size = 50 });
        return RenderDocuments(document);
    }

    private Document CreateDocument(DateOnly month)
    {
        var document = new Document();

        document.Info.Title = $"{ResourceReportGenerationMessages.EXPENSES_FOR} {month:Y}";
        document.Info.Author = "Davi da Silva";

        var style = document.Styles["Normal"];
        style!.Font.Name = FontHelper.RALEWAY_REGULAR;

        return document;
    }

    private Section CreatePage(Document document)
    {
        var section = document.AddSection();
        section.PageSetup = document.DefaultPageSetup.Clone();
        section.PageSetup.PageFormat = PageFormat.A4;
        section.PageSetup.LeftMargin = 40;
        section.PageSetup.RightMargin = 40;
        section.PageSetup.TopMargin = 80;
        section.PageSetup.BottomMargin = 80;

        return section;
    }

    private byte[] RenderDocuments(Document document)
    {
        var renderer = new PdfDocumentRenderer
        {
            Document = document,

        };

        renderer.RenderDocument();

        using var file = new MemoryStream();
        renderer.PdfDocument.Save(file);

        return file.ToArray();
    }
}
