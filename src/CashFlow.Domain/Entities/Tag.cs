namespace CashFlow.Domain.Entities;
public class Tag
{
    public long Id { get; set; }
    public Enums.Tag Value { get; set; } // Para não causar erro com o Tag da Classe.
    public long ExpenseId { get; set; }
    public Expense Expense { get; set; } = default!;
}
