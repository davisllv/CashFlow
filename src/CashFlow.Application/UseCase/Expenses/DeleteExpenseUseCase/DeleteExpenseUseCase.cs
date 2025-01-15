
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception.ExceptionBase;
using CashFlow.Exception;

namespace CashFlow.Application.UseCase.Expenses.DeleteExpenseUseCase;

public class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    private readonly IExpenseWriteOnlyRepository _expenseWriteOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteExpenseUseCase(IExpenseWriteOnlyRepository expenseWriteOnlyRepository, IUnitOfWork unitOfWork)
    {
        _expenseWriteOnlyRepository = expenseWriteOnlyRepository;
        _unitOfWork = unitOfWork;
    }
    public async Task Execute(long id)
    {

        var result = await _expenseWriteOnlyRepository.Delete(id);

        if (!result)
            throw new NotFoundException(ResourceErrorMessages.EXPENSE_NOT_FOUND);

        await _unitOfWork.Commit();
    }
}
