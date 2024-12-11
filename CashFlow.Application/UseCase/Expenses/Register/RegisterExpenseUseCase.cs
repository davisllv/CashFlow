using CashFlow.Communication.Enums;
using CashFlow.Communication.Request;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCase.Expenses.Register;

public class RegisterExpenseUseCase
{
    public ResponseRegisterExpenseJson Execute(RequestRegisterExpenseJson request)
    {
        // To do - Validation
        Validate(request);
        return new ResponseRegisterExpenseJson();
    }

    private void Validate(RequestRegisterExpenseJson request)
    {
        var titleIsEmpty = string.IsNullOrWhiteSpace(request.Title); // Ele pega inclusive os valores com espaços
        if(titleIsEmpty)
            throw new ArgumentException("The title is required");

        if(request.Amount <= 0)
            throw new ArgumentException("The amout must be greater than zero");

        var result = DateTime.Compare(request.Date, DateTime.UtcNow);
        if (result > 0)
            throw new ArgumentException("Expenses cannot be for the future");

        if (!Enum.IsDefined(typeof(PaymentType), request.PaymentType)) // Forma de validar o formato de um enum;
            throw new ArgumentException("Payment type is not valid");
    }
}
