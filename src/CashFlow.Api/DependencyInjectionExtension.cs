using CashFlow.Application.UseCase.Expenses.Register;
using CashFlow.Domain.Repositories.Expenses;

namespace CashFlow.Infrastructure;

// Classe precisa ser Statica e o método que será implementado também precisa ser státic.
public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {

        services.AddScoped<IRegisterExpenseUseCase, RegisterExpenseUseCase>();
    }
}
