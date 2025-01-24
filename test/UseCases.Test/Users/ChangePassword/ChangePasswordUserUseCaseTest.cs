using CashFlow.Application.UseCase.Users.ChangePassword;
using CashFlow.Communication.Request;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Services.LoggedUser;
using CommomTestUtilities.Cryptography;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Repositories.User;
using CommomTestUtilities.Request;
using FluentAssertions;

namespace UseCases.Test.Users.ChangePassword;
public class ChangePasswordUserUseCaseTest
{
    public ChangePasswordUserUseCase CreateUseCase(User user, string? password = null)
    {
        ILoggedUser loggedUser = new LoggedUserBuilder().Get(user).Build();
        IUnitOfWork unitOfWork = UnitOfWorkBuilder.Build();
        IUserUpdateOnlyRepository userUpdateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().Build();

        PasswordEncripterBuilder passwordEncripter = new PasswordEncripterBuilder();

        if(!string.IsNullOrWhiteSpace(password))
            passwordEncripter.Verify(password);

        return new ChangePasswordUserUseCase(loggedUser, unitOfWork, userUpdateOnlyRepository, passwordEncripter.Build());
    }
    [Fact]
    public async Task Sucess()
    {
        User user = UserBuilder.Build();

        ChangePasswordUserUseCase useCase = CreateUseCase(user);

        RequestChangePasswordJson request = RequestChangePasswordJsonBuilder.Build();


        var act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Password_Empty()
    {
        User user = UserBuilder.Build();

        ChangePasswordUserUseCase useCase = CreateUseCase(user);

        RequestChangePasswordJson request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = string.Empty;


        var act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();
    }

}
