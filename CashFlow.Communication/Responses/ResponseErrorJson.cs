namespace CashFlow.Communication.Responses;

public class ResponseErrorJson
{
    public ResponseErrorJson(string error)
    {
        ErrorMessage = [error];
    }

    public ResponseErrorJson(List<string> error)
    {
        ErrorMessage = error;
    }

    public List<string> ErrorMessage {  get; set; } // Para não esquecer de definir uma mensagem de erro; Eu posso colocar o required ou definir um construtor
}
