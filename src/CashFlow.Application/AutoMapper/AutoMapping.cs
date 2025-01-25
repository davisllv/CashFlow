using AutoMapper;
using CashFlow.Communication.Request;
using CashFlow.Communication.Responses.Expenses;
using CashFlow.Communication.Responses.Users;
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
        CreateMap<Expense, ResponseExpenseJson>()
            .ForMember(dest => dest.Tags, config => config.MapFrom(source => source.Tags.Select(tag => tag.Value)));
        CreateMap<Expense, ResponseRegisterExpenseJson>();
        CreateMap<Expense, ResponseShortExpenseJson>();
        CreateMap<User, ResponseUserProfileJson>();

    }

    private void RequestToEntity()
    {
        // A origem, Destino
        CreateMap<RequestRegisterUserJson, User>()
            .ForMember(dest => dest.Password, config => config.Ignore()); // Forma de ignorar a variável

        CreateMap<RequestExpenseJson, Expense>()
            .ForMember(dest => dest.Tags, config => config.MapFrom(source => source.Tags.Distinct()));


        CreateMap<Communication.Enums.Tag, Tag>()
            .ForMember(dest => dest.Value, config => config.MapFrom(source => source)); // Forma de fazer o mapeamento dos valores.
    }
}
