using CashFlow.Communication.Request;
using CashFlow.Communication.Responses;
using CashFlow.Exception.ExceptionBase;
using CashFlow.Infrastructure.DataAccess;
using FluentValidation.Results;

namespace CashFlow.Application.UseCase.Expenses.Register;

public class RegisterExpenseUseCase
{
    public ResponseRegisterExpenseJson Execute(RequestRegisterExpenseJson request)
    {
        // To do - Validation
        Validate(request);

        var dbContext = new CashFlowDbContext();

        var entity = new Domain.Entities.Expenses
        {
            Amount = request.Amount,
            Date = request.Date,
            Description = request.Description,
            Title = request.Title,
        };

        return new ResponseRegisterExpenseJson();
    }

    private void Validate(RequestRegisterExpenseJson request)
    {
        ValidationResult validation = new RegisterExpenseValidator().Validate(request);
        if (!validation.IsValid)
        {
            var errorMessages = validation.Errors.Select(p => p.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
            
    }
}
