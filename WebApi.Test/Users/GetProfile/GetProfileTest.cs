using FluentAssertions;
using System.Net;
using System.Text.Json;
using WebApi.Test.Resources;

namespace WebApi.Test.Users.GetProfile;
public class GetProfileTest : CashFlowClassFixture
{
    private const string METHOD = "api/Users";
    private readonly string _token;
    private readonly string _name;
    private readonly string _email;


    public GetProfileTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _name = webApplicationFactory.User_Team_Member.GetName();
        _email = webApplicationFactory.User_Team_Member.GetEmail();
    }

    [Fact]
    public async Task Sucess()
    {
        var result = await DoGet(requestUri: METHOD, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().Should().Be(_name);
        response.RootElement.GetProperty("email").GetString().Should().Be(_email);

    }
}
