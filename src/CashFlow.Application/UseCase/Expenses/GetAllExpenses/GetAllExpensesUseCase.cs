using AutoMapper;
using CashFlow.Communication.Responses.Expenses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCase.Expenses.GetAllExpenses;

public class GetAllExpensesUseCase : IGetAllExpensesUseCase
{
    private readonly IExpenseReadOnlyRepository _repository;
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;
    public GetAllExpensesUseCase(IExpenseReadOnlyRepository repository, IMapper mapper, ILoggedUser loggedUser)
    {
        _repository = repository;
        _mapper = mapper;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseExpensesJson> Execute()
    {
        var userLogged = _loggedUser.Get();
        List<Expense> result = await _repository.GetAll(userLogged);
        return new ResponseExpensesJson
        {
            Expenses = _mapper.Map<List<ResponseShortExpenseJson>>(result)
        };
    }
}
