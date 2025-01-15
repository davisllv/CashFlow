using CashFlow.Communication.Request;
using CashFlow.Communication.Responses.Users;

namespace CashFlow.Application.UseCase.Users.Register;
public interface IRegisterUserUseCase
{
    public Task<ResponseRegisterUserJson> Execute(RequestUserJson request);
}
