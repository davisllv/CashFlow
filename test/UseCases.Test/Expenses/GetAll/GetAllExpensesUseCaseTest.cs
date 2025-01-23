using AutoMapper;
using CashFlow.Application.UseCase.Expenses.GetAllExpenses;
using CashFlow.Communication.Responses.Expenses;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Mapper;
using CommomTestUtilities.Repositories.Expense;
using FluentAssertions;

namespace UseCases.Test.Expenses.GetAll;
public class GetAllExpensesUseCaseTest
{
    public GetAllExpensesUseCase CreateUseCase()
    {
        User user = UserBuilder.Build();
        List<Expense> expenses = ExpensesBuilder.Collection(user);
        ILoggedUser loggedUser = new LoggedUserBuilder().Get(user).Build();
        IMapper mapper = MapperBuilder.Build();
        IExpenseReadOnlyRepository expenseReadOnlyRepository = new ExpenseReadOnlyRepositoryBuilder().GetAll(user, expenses).Build();

        return new GetAllExpensesUseCase(expenseReadOnlyRepository, mapper, loggedUser);
    }

    [Fact]
    public async Task Sucess()
    {
        GetAllExpensesUseCase useCase = CreateUseCase();

        ResponseExpensesJson response = await useCase.Execute();

        response.Should().NotBeNull();
        response.Expenses.Should().NotBeNullOrEmpty().And.AllSatisfy(expense =>
        {
            expense.Id.Should().BeGreaterThan(0);
            expense.Title.Should().NotBeNull();
            expense.Amount.Should().BeGreaterThan(0);
        });
    }
}
