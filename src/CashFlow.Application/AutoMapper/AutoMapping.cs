using AutoMapper;
using CashFlow.Communication.Request;
using CashFlow.Communication.Responses.Expenses;
using CashFlow.Domain.Entities;

namespace CashFlow.Application.AutoMapper;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityToResponse();
    }

    private void EntityToResponse()
    {
        CreateMap<Expense, ResponseRegisterExpenseJson>();
        CreateMap<Expense, ResponseShortExpenseJson>();
        CreateMap<Expense, ResponseExpenseJson>();

    }

    private void RequestToEntity()
    {
        // A origem, Destino
        CreateMap<RequestExpenseJson, Expense>();
        CreateMap<RequestUserJson, User>()
            .ForMember(dest => dest.Password, config => config.Ignore()); // Forma de ignorar a variável
    }
}
