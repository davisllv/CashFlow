using CashFlow.Application.UseCase.Login.DoLogin;
using CashFlow.Communication.Request;
using CashFlow.Communication.Responses.Users;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CommomTestUtilities.Cryptography;
using CommomTestUtilities.Entities;
using CommomTestUtilities.Repositories.User;
using CommomTestUtilities.Request;
using CommomTestUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.DoLogin;
public class DoLoginUseCaseTest
{
    private DoLoginUseCase CreateUseCase(User user, string? password = null)
    {
        var passwordEncripter = new PasswordEncripterBuilder().Verify(password).Build();
        IAcessTokenGenerator acessTokenGenerator = AcessTokenGeneratorBuilder.Build();

        var readRepository = new UserReadOnlyRepositoryBuilder().GetUserByEmail(user).Build();


        return new DoLoginUseCase(readRepository, passwordEncripter, acessTokenGenerator);
    }

    [Fact]
    public async Task Sucess()
    {
        User user = UserBuilder.Build();
        RequestLoginJson request = RequestDoLoginJsonBuilder.Build();
        request.Email = user.Email;

        DoLoginUseCase useCase = CreateUseCase(user, request.Password);
        

        ResponseRegisterUserJson result = await useCase.Execute(request);
        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();

    }
    [Fact]
    public async Task Error_User_Not_Found()
    {
        RequestLoginJson request = RequestDoLoginJsonBuilder.Build();
        User user = UserBuilder.Build();
        DoLoginUseCase useCase = CreateUseCase(user);
        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<InvalidLoginException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID));

    }
    [Fact]
    public async Task Error_Password_Invalid()
    {
        RequestLoginJson request = RequestDoLoginJsonBuilder.Build();
        User user = UserBuilder.Build();
        DoLoginUseCase useCase = CreateUseCase(user);
        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<InvalidLoginException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID));

    }
}
