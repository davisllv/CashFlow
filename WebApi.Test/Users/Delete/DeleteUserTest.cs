using FluentAssertions;
using System.Net;

namespace WebApi.Test.Users.Delete;
public class DeleteUserTest : CashFlowClassFixture
{
    private const string METHOD = "api/Users";
    private readonly string _token;
    public DeleteUserTest(CustomWebApplicationFactory customWebApplication) : base(customWebApplication)
    {
        _token = customWebApplication.User_Team_Member.GetToken();
    }

    [Fact]
    public async Task Sucess()
    {
        var result = await DoDelete(requestUri: METHOD, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
}
