using CashFlow.Communication.Request;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Repositories;
using AutoMapper;
using CashFlow.Exception.ExceptionBase;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCase.Expenses.UpdateExpenseUseCase;

public class UpdateExpenseUseCase : IUpdateExpenseUseCase
{
    private readonly IExpenseUpdateOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public UpdateExpenseUseCase(IExpenseUpdateOnlyRepository repository, IUnitOfWork unitOfWork, IMapper mapper, ILoggedUser loggedUser)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }
    public async Task Execute(long id, RequestExpenseJson request)
    {
        Validate(request);
        User loggedUser = await _loggedUser.Get();

        Expense? expense = await _repository.GetById(loggedUser, id);

        if (expense is null)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        _mapper.Map(request, expense);


        _repository.Update(expense);

        await _unitOfWork.Commit();
    }

    private void Validate(RequestExpenseJson request)
    {
        var validation = new ExpenseValidator().Validate(request);

        if (!validation.IsValid)
        {
            var errorMessages = validation.Errors.Select(p => p.ErrorMessage).ToList();

            throw new ErrorOnValidationException(errorMessages);
        }
    }
}
