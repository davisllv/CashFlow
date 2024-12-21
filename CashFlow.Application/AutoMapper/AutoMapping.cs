using AutoMapper;
using CashFlow.Communication.Request;
using CashFlow.Communication.Responses;
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
        // A origem, Destino
        CreateMap<RequestRegisterExpenseJson, Expense>();
    }

    private void RequestToEntity()
    {
        CreateMap<Expense, ResponseRegisterExpenseJson>();
    }
}
