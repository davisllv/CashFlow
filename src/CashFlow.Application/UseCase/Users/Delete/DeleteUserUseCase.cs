using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCase.Users.Delete;
public class DeleteUserUseCase : IDeleteUserUseCase
{
    private readonly IUserWriteOnlyRepository _repository;
    private readonly ILoggedUser _loggedUser;
    private readonly IUnitOfWork _unitOfWork;    
    public DeleteUserUseCase(IUserWriteOnlyRepository repository, ILoggedUser loggedUser, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _loggedUser = loggedUser;
        _unitOfWork = unitOfWork;
    }

    public async Task Execute()
    {
        User userLogged = await _loggedUser.Get();

        await _repository.Delete(userLogged.Id);

        await _unitOfWork.Commit();
    }
}
