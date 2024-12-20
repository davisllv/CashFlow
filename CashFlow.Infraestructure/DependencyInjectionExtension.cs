using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Infrastructure.DataAccess.Respositories;
using Microsoft.Extensions.DependencyInjection;

namespace CashFlow.Infrastructure;

// Classe precisa ser Statica e o método que será implementado também precisa ser státic.
public static class DependencyInjectionExtension
{
    public static void AddInfrastructure(this IServiceCollection services)
    {

        services.AddScoped<IExpensesRepository, ExpensesRepository>();
    }
}
