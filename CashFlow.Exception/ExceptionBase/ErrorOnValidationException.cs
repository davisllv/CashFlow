namespace CashFlow.Exception.ExceptionBase;

public class ErrorOnValidationException : CashFlowException
{
    public List<string> Errors { get; set; }
    public ErrorOnValidationException(List<string> errorMessages) : base(string.Empty) // Forma para repassar o valor para o construtor
    {
        Errors = errorMessages;
    }
}
