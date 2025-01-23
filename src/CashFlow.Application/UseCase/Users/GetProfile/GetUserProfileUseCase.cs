using AutoMapper;
using CashFlow.Communication.Responses.Users;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Services.LoggedUser;

namespace CashFlow.Application.UseCase.Users.GetProfile;
public class GetUserProfileUseCase : IGetUserProfileUseCase
{
    private readonly IMapper _mapper;
    private readonly ILoggedUser _loggedUser;

    public GetUserProfileUseCase(IMapper mapper, ILoggedUser loggedUser)
    {
        _mapper = mapper;
        _loggedUser = loggedUser;
    }
    public async Task<ResponseUserProfileJson> Execute()
    {
        User user = await _loggedUser.Get();

        return _mapper.Map<ResponseUserProfileJson>(user);
    }
}
