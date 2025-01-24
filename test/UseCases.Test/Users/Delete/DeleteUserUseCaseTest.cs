using CashFlow.Application.UseCase.Users.Delete;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Services.LoggedUser;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Repositories.User;
using FluentAssertions;

namespace UseCases.Test.Users.Delete;
public class DeleteUserUseCaseTest
{
    public DeleteUserUseCase CreateUseCase(User user)
    {
        IUserWriteOnlyRepository repository = UserWriteOnlyRepositoryBuilder.Build();
        ILoggedUser loggedUser = new LoggedUserBuilder().Get(user).Build();
        IUnitOfWork unitOfWork = UnitOfWorkBuilder.Build();

        return new DeleteUserUseCase(repository, loggedUser, unitOfWork);
    }
    [Fact]
    public async void Sucess()
    {
        User user = UserBuilder.Build();

        DeleteUserUseCase useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute();

        await act.Should().NotThrowAsync();
    }
}
