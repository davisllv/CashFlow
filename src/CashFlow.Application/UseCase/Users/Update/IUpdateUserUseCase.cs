using CashFlow.Communication.Request;

namespace CashFlow.Application.UseCase.Users.Update;
public interface IUpdateUserUseCase
{
    Task Execute(RequestUpdateUserJson request);
}
