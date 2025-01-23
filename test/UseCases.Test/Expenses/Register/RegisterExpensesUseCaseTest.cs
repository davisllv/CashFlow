using AutoMapper;
using CashFlow.Application.UseCase.Expenses.Register;
using CashFlow.Communication.Request;
using CashFlow.Communication.Responses.Expenses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories;
using CommomTestUtilities.Repositories.Expense;
using CommomTestUtilities.Request;
using FluentAssertions;

namespace UseCases.Test.Expenses.Register;
public class RegisterExpensesUseCaseTest
{
    public RegisterExpenseUseCase CreateUseCase()
    {
        User user = UserBuilder.Build();
        IMapper mapper = MapperBuilder.Build();
        IUnitOfWork unitOfWork = UnitOfWorkBuilder.Build();
        IExpenseWriteOnlyRepository repository = ExpenseWriteOnlyRepositoryBuilder.Build();
        ILoggedUser loggedUser = new LoggedUserBuilder().Get(user).Build();

        return new RegisterExpenseUseCase(repository, unitOfWork, mapper, loggedUser);
    }

    [Fact]
    public async Task Sucess()
    {
        RegisterExpenseUseCase useCase = CreateUseCase();
        RequestExpenseJson request = RequestExpenseJsonBuilder.Build();

        ResponseRegisterExpenseJson result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Title.Should().Be(request.Title);
    }
    [Fact]
    public async Task Error_Title_Empty()
    {
        RegisterExpenseUseCase useCase = CreateUseCase();
        RequestExpenseJson request = RequestExpenseJsonBuilder.Build();
        request.Title = string.Empty;

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidationException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.TITLE_REQUIRED));
    }
}
