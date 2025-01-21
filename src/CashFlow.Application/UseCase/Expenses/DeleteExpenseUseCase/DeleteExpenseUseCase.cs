
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionBase;
using CashFlow.Exception;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCase.Expenses.DeleteExpenseUseCase;

public class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    private readonly IExpenseReadOnlyRepository _expenseReadOnlyRepository;
    private readonly IExpenseWriteOnlyRepository _expenseWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;
    public DeleteExpenseUseCase(IExpenseReadOnlyRepository expenseReadOnlyRepository, IExpenseWriteOnlyRepository expenseWriteOnlyRepository, IUnitOfWork unitOfWork, ILoggedUser loggedUser)
    {
        _expenseReadOnlyRepository = expenseReadOnlyRepository;
        _expenseWriteOnlyRepository = expenseWriteOnlyRepository;
        _unitOfWork = unitOfWork;
        _loggedUser = loggedUser;
    }
    public async Task Execute(long id)
    {
        var userLogged = await _loggedUser.Get();
        var expense = await _expenseReadOnlyRepository.GetById(userLogged, id);

        if (expense is null)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        await _expenseWriteOnlyRepository.Delete(id);


        await _unitOfWork.Commit();
    }
}
