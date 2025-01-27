using AutoMapper;
using CashFlow.Application.UseCase.Expenses.GetExpenseById;
using CashFlow.Communication.Responses.Expenses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories.Expense;
using FluentAssertions;

namespace UseCases.Test.Expenses.GetById;
public class GetExpenseByIdUseCaseTest
{
    public GetExpenseByIdUseCase CreateUseCase(User user, Expense expense)
    {
        IMapper mapper = MapperBuilder.Build();
        ILoggedUser loggedUser = new LoggedUserBuilder().Get(user).Build();
        IExpenseReadOnlyRepository expenseReadOnly = new ExpenseReadOnlyRepositoryBuilder().GetById(user, expense).Build();

        return new GetExpenseByIdUseCase(expenseReadOnly, mapper, loggedUser);
    }
    [Fact]
    public async Task Sucess()
    {
        User user = UserBuilder.Build();
        Expense expense = ExpensesBuilder.Build(user);

        GetExpenseByIdUseCase useCase = CreateUseCase(user, expense);

        ResponseExpenseJson response = await useCase.Execute(expense.Id);

        response.Should().NotBeNull();
        response.Id.Should().Be(expense.Id);
        response.Title.Should().Be(expense.Title);
        response.Description.Should().Be(expense.Description);
        response.Amount.Should().Be(expense.Amount);
        response.PaymentType.Should().Be((CashFlow.Communication.Enums.PaymentType)expense.PaymentType);
        response.Tags.Should().NotBeNullOrEmpty().And.BeEquivalentTo(expense.Tags.Select(tag=> tag.Value));
    }
}
