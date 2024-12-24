using CashFlow.Application.AutoMapper;
using CashFlow.Application.UseCase.Expenses.DeleteExpenseUseCase;
using CashFlow.Application.UseCase.Expenses.GetAllExpenses;
using CashFlow.Application.UseCase.Expenses.GetExpenseById;
using CashFlow.Application.UseCase.Expenses.Register;
using CashFlow.Application.UseCase.Expenses.Reports;
using CashFlow.Application.UseCase.Expenses.UpdateExpenseUseCase;
using Microsoft.Extensions.DependencyInjection;

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
        services.AddScoped<IGetAllExpensesUseCase, GetAllExpensesUseCase>();
        services.AddScoped<IGetExpenseByIdUseCase, GetExpenseByIdUseCase>();
        services.AddScoped<IDeleteExpenseUseCase, DeleteExpenseUseCase>();
        services.AddScoped<IUpdateExpenseUseCase, UpdateExpenseUseCase>();

        services.AddScoped<IGenerateExpensesReportExcelUseCase, GenerateExpensesReportExcelUseCase>();
    }
}
