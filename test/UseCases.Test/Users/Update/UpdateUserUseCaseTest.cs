using CashFlow.Application.UseCase.Users.Update;
using CashFlow.Communication.Request;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception.ExceptionBase;
using CashFlow.Exception;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Repositories.User;
using CommomTestUtilities.Request;
using FluentAssertions;

namespace UseCases.Test.Users.Update;
public class UpdateUserUseCaseTest
{
    private UpdateUserUseCase CreateUseCase(User user, string? email = null)
    {
        IUserUpdateOnlyRepository userUpdateOnlyRepository = new UserUpdateOnlyRepositoryBuilder().GetById(user).Build();
        UserReadOnlyRepositoryBuilder userReadOnlyRepository = new UserReadOnlyRepositoryBuilder();

        if(!string.IsNullOrWhiteSpace(email))
            userReadOnlyRepository.ExistActiveUserWithEmail(email);

        ILoggedUser loggedUser = new LoggedUserBuilder().Get(user).Build();
        IUnitOfWork unitOfWork = UnitOfWorkBuilder.Build();
        return new UpdateUserUseCase(userReadOnlyRepository.Build(), userUpdateOnlyRepository, loggedUser, unitOfWork);
    }

    [Fact]
    public async Task Sucess()
    {
        User user = UserBuilder.Build();
        UpdateUserUseCase useCase = CreateUseCase(user);

        RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();

        var act = async () => await useCase.Execute(request);
        await act.Should().NotThrowAsync();

        user.Name.Should().Be(request.Name);
        user.Email.Should().Be(request.Email);
    }

    [Fact]
    public async Task Error_Name_Empty()
    {
        User user = UserBuilder.Build();
        UpdateUserUseCase useCase = CreateUseCase(user);

        RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.NAME_EMPTY));
    }

    [Fact]
    public async Task Error_Email_Registered()
    {
        User user = UserBuilder.Build();
        RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();

        UpdateUserUseCase useCase = CreateUseCase(user, request.Email);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_ALREADY_REGISTER));
    }
}
