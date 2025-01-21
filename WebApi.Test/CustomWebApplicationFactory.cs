using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Infrastructure.DataAccess;
using CommomTestUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace WebApi.Test;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    private CashFlow.Domain.Entities.User _user;
    private string _password;
    private string _token;
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

                StartDatabase(dbContext, passwordEncripter);

                _token = tokenGenerator.Generate(_user);
            });
    }

    public string GetEmail() => _user.Email;
    public string GetName() => _user.Name;
    public string GetPassword() => _password;
    public string GetToken() => _token;



    private void StartDatabase(CashFlowDbContext dbContext, IPasswordEncripter passwordEncripter)
    {
        _user = UserBuilder.Build();
        _password = _user.Password;
        _user.Password = passwordEncripter.Encrypt(_user.Password);
        dbContext.Users.Add(_user);

        dbContext.SaveChanges();
    }
}
