using CashFlow.Communication.Request;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Entities;
using CashFlow.Exception.ExceptionBase;
using FluentValidation.Results;
using CashFlow.Domain.Repositories;
using AutoMapper;

namespace CashFlow.Application.UseCase.Expenses.Register;

public class RegisterExpenseUseCase : IRegisterExpenseUseCase
{
    private readonly IExpenseWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    public RegisterExpenseUseCase(IExpenseWriteOnlyRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<ResponseRegisterExpenseJson> Execute(RequestExpenseJson request)
    {
        // To do - Validation - Verified Commit
        Validate(request);

        Expense expense = _mapper.Map<Expense>(request);

        await _repository.Add(expense);

        await _unitOfWork.Commit();

        return _mapper.Map<ResponseRegisterExpenseJson>(expense);
    }

    private void Validate(RequestExpenseJson request)
    {
        ValidationResult validation = new ExpenseValidator().Validate(request);
        if (!validation.IsValid)
        {
            var errorMessages = validation.Errors.Select(p => p.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
            
    }
}
