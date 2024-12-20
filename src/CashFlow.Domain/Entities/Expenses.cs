using CashFlow.Communication.Enums;

namespace CashFlow.Domain.Entities;
// Espelho da Tabela do Banco de Dados.
public class Expenses
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Decimal Amount { get; set; }
    public DateTime Date { get; set; }
    public PaymentType PaymentType { get; set; }
}
