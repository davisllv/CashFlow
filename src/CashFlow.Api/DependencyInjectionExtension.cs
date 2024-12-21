using CashFlow.Application.AutoMapper;
using CashFlow.Application.UseCase.Expenses.Register;

namespace CashFlow.Infrastructure;

// Classe precisa ser Statica e o método que será implementado também precisa ser státic.
public static class DependencyInjectionExtension
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddAutoMapper(services);
        AddUseCase(services);

    }

    public static void AddAutoMapper(IServiceCollection services)
    {
        services.AddAutoMapper(typeof(AutoMapping));
    }

    public static void AddUseCase(IServiceCollection services)
    {
        services.AddScoped<IRegisterExpenseUseCase, RegisterExpenseUseCase>();
    }
}
