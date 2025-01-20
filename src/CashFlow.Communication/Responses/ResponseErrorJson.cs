namespace CashFlow.Communication.Responses;

public class ResponseErrorJson
{
    public ResponseErrorJson(string error)
    {
        ErrorMessages = [error];
    }

    public ResponseErrorJson(List<string> error)
    {
        ErrorMessages = error;
    }

    public List<string> ErrorMessages {  get; set; } // Para não esquecer de definir uma mensagem de erro; Eu posso colocar o required ou definir um construtor
}
