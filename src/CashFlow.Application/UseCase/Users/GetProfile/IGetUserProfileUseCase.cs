using CashFlow.Communication.Responses.Users;

namespace CashFlow.Application.UseCase.Users.GetProfile;
public interface IGetUserProfileUseCase
{
    public Task<ResponseUserProfileJson> Execute();
}
