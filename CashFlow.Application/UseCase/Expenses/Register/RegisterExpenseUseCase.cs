using CashFlow.Communication.Request;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Entities;
using CashFlow.Exception.ExceptionBase;
using FluentValidation.Results;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Repositories;

namespace CashFlow.Application.UseCase.Expenses.Register;

public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IExpensesRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    public RegisterExpenseUseCase(IExpensesRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<ResponseRegisterExpenseJson> Execute(RequestRegisterExpenseJson request)
    {
        // To do - Validation
        Validate(request);

        Expense expense = new Expense
        {
            Amount = request.Amount,
            Date = request.Date,
            Description = request.Description,
            Title = request.Title,
            PaymentType = (PaymentType)request.PaymentType,
        };

        await _repository.Add(expense);

        await _unitOfWork.Commit();

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
