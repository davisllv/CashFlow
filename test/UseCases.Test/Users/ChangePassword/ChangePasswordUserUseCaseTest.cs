using CashFlow.Application.UseCase.Users.ChangePassword;
using CashFlow.Communication.Request;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception.ExceptionBase;
using CashFlow.Exception;
using CommomTestUtilities.Cryptography;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Repositories.User;
using CommomTestUtilities.Request;
using FluentAssertions;
using CashFlow.Domain.Security.Cryptography;

namespace UseCases.Test.Users.ChangePassword;
public class ChangePasswordUserUseCaseTest
{
    public ChangePasswordUserUseCase CreateUseCase(User user, string? password = null)
    {
        ILoggedUser loggedUser = new LoggedUserBuilder().Get(user).Build();
        IUnitOfWork unitOfWork = UnitOfWorkBuilder.Build();
        IUserUpdateOnlyRepository userUpdateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();

        IPasswordEncripter passwordEncripter = new PasswordEncripterBuilder().Verify(password).Build();

        return new ChangePasswordUserUseCase(loggedUser, unitOfWork, userUpdateOnlyRepository, passwordEncripter);
    }
    [Fact]
    public async Task Sucess()
    {
        User user = UserBuilder.Build();

        RequestChangePasswordJson request = RequestChangePasswordJsonBuilder.Build();
        ChangePasswordUserUseCase useCase = CreateUseCase(user, request.Password);

        var act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();
    }

    [Fact]
    public async Task Error_Password_Empty()
    {
        User user = UserBuilder.Build();

        RequestChangePasswordJson request = RequestChangePasswordJsonBuilder.Build();
        ChangePasswordUserUseCase useCase = CreateUseCase(user, request.Password);

        request.NewPassword = string.Empty;


        var act = async () => { await useCase.Execute(request); };

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(e => e.GetErrors().Count == 1 &&
                e.GetErrors().Contains(ResourceErrorMessages.INVALID_PASSWORD));
    }


    [Fact]
    public async Task Error_CurrentPassword_Different()
    {
        var user = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => { await useCase.Execute(request); };

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(e => e.GetErrors().Count == 1 &&
                e.GetErrors().Contains(ResourceErrorMessages.PASSWORD_DIFERENT));
    }

}
