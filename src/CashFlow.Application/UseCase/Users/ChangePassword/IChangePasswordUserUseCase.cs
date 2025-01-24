using CashFlow.Communication.Request;

namespace CashFlow.Application.UseCase.Users.ChangePassword;
public interface IChangePasswordUserUseCase
{
    Task Execute(RequestChangePasswordJson request);
}
