using AutoMapper;
using CashFlow.Application.UseCase.Users.GetProfile;
using CashFlow.Communication.Responses.Users;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Services.LoggedUser;
using CommomTestUtilities.Entities;
using CommomTestUtilities.LoggedUser;
using CommomTestUtilities.Mapper;
using FluentAssertions;

namespace WebApi.Test.Users.GetProfile;
public class GetUserProfileTest
{
    public GetUserProfileUseCase CreateUseCase(User user)
    {
        IMapper mapper = MapperBuilder.Build();
        ILoggedUser loggedUser = new LoggedUserBuilder().Get(user).Build();

        return new GetUserProfileUseCase(mapper, loggedUser);
    }

    [Fact]
    public async Task Sucess()
    {
        User user = UserBuilder.Build();

        GetUserProfileUseCase useCase = CreateUseCase(user);

        ResponseUserProfileJson response = await useCase.Execute();

        response.Should().NotBeNull();
        response.Name.Should().Be(user.Name);
        response.Email.Should().Be(user.Email); 


    }
}
