using CashFlow.Communication.Request;
using CashFlow.Exception;
using CommomTestUtilities.Request;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Login.DoLogin;
public class DoLoginTest : CashFlowClassFixture
{
    private const string METHOD = "api/Login";
    private readonly string _email;
    private readonly string _name;
    private readonly string _password;

    public DoLoginTest(CustomWebApplicationFactory applicationFactory) : base(applicationFactory)
    {
        _email = applicationFactory.User_Team_Member.GetEmail();
        _name = applicationFactory.User_Team_Member.GetName();
        _password = applicationFactory.User_Team_Member.GetPassword();
    }

    [Fact]
    public async Task Sucess()
    {
        RequestLoginJson request = new RequestLoginJson()
        {
            Email = _email,
            Password = _password
        };

        var result = await DoPost(requestUri: METHOD, request: request);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseBody = await result.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);
        // GetProperty - Porque é um documento json.
        responseData.RootElement.GetProperty("name").GetString().Should().Be(_name);
        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Invalid_Login(string cultureInfo)
    {
        RequestLoginJson request = RequestDoLoginJsonBuilder.Build()    ;

        var result = await DoPost(METHOD, request, cultureInfo: cultureInfo);

        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);

        var responseBody = await result.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        var errors = responseData.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(cultureInfo));
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectMessage));
    }
}
