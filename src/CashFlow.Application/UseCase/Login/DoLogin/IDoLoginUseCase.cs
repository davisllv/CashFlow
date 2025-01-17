using CashFlow.Communication.Request;
using CashFlow.Communication.Responses.Users;

namespace CashFlow.Application.UseCase.Login.DoLogin;
public interface IDoLoginUseCase
{
    Task<ResponseRegisterUserJson> Execute(RequestLoginJson request);
}
