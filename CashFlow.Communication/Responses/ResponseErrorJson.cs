namespace CashFlow.Communication.Responses;

public class ResponseErrorJson
{
    public ResponseErrorJson(string error)
    {
        ErrorMessage = error;
    }
    public string ErrorMessage {  get; set; } = string.Empty; // Para não esquecer de definir uma mensagem de erro; Eu posso colocar o required ou definir um construtor
}
