using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infrastructure.DataAccess;
using CommomTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    // Private set porque somente dentro dessa classe eu vou poder mexer nesse cara
    public UserIdentityManager User_Team_Member { get; private set; } = default!;
    public UserIdentityManager User_Admin { get; private set; } = default!;// Forma de definir que os valores não serão nulos

    public ExpenseIdentityManager Expenses_MemberTeam { get; private set; } = default!; 
    public ExpenseIdentityManager Expenses_Admin { get; private set; } = default!; 


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        // Preciso deixar o CashFlowDbContext Public para ter acesso aqui.
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var serviceProvider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();

                services.AddDbContext<CashFlowDbContext>(conf =>
                {
                    conf.UseInMemoryDatabase("InMemoryDbForTesting");
                    conf.UseInternalServiceProvider(serviceProvider);
                });

                var scope = services.BuildServiceProvider().CreateScope();
                CashFlowDbContext dbContext = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>();
                IPasswordEncripter passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEncripter>();
                IAcessTokenGenerator tokenGenerator = scope.ServiceProvider.GetRequiredService<IAcessTokenGenerator>();

                StartDatabase(dbContext, passwordEncripter, tokenGenerator);

            });
    }

    private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter, IAcessTokenGenerator tokenGenerator)
    {
        var userTeamMember = AddUserTeamMember(dbContext, passwordEncripter, tokenGenerator);
        Expenses_MemberTeam = new ExpenseIdentityManager(AddExpenses(dbContext, userTeamMember, expenseId: 1));

        var userAdmin = AddUserAdmin(dbContext, passwordEncripter, tokenGenerator);
        Expenses_Admin = new ExpenseIdentityManager(AddExpenses(dbContext, userAdmin, expenseId: 2));

        dbContext.SaveChanges();
    }

    private User AddUserTeamMember(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter, IAcessTokenGenerator tokenGenerator)
    {
        var user = UserBuilder.Build();
        user.Id = 1;

        var password = user.Password;
        user.Password = passwordEncripter.Encrypt(user.Password);

        dbContext.Users.Add(user);

        var token = tokenGenerator.Generate(user);

        User_Team_Member = new UserIdentityManager(user, password, token);

        return user;

    }
    private User AddUserAdmin(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter, IAcessTokenGenerator tokenGenerator)
    {
        var user = UserBuilder.Build(Roles.ADMIN);
        user.Id = 2;

        var password = user.Password;
        user.Password = passwordEncripter.Encrypt(user.Password);

        dbContext.Users.Add(user);

        var token = tokenGenerator.Generate(user);

        User_Admin = new UserIdentityManager(user, password, token);

        return user;

    }

    private Expense AddExpenses(CashFlowDbContext dbContext, User user, long expenseId)
    {
        var expenses = ExpensesBuilder.Build(user);
        expenses.Id = expenseId;
        dbContext.Expenses.Add(expenses);

        return expenses;

    }
}
