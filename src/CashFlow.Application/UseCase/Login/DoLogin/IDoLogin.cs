using CashFlow.Communication.Responses.Users;

namespace CashFlow.Application.UseCase.Login.DoLogin;
public interface IDoLogin
{
    Task<ResponseRegisterUserJson> Execute();
}
