namespace CashFlow.Exception.ExceptionBase;

public abstract class CashFlowException : SystemException
{
    protected CashFlowException(string message) : base(message)
    {
        
    }

    public abstract int StatusCode { get; } // Abstract porque eu vou sobrescrever e eu não posso escrever nenhum método
    public abstract List<string> GetErrors();
}
