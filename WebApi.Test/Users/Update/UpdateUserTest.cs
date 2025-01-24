using CashFlow.Communication.Request;
using CashFlow.Exception;
using CommomTestUtilities.Request;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Update;
public class UpdateUserTest : CashFlowClassFixture
{
    private const string METHOD = "api/Users";
    private readonly string _token;
    private readonly string _emailAdmin;
    public UpdateUserTest(CustomWebApplicationFactory customWebApplicationFactory) : base(customWebApplicationFactory)
    {
        _token = customWebApplicationFactory.User_Team_Member.GetToken();
        _emailAdmin = customWebApplicationFactory.User_Admin.GetEmail();
    }

    [Fact]
    public async Task Sucess()
    {
        RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();

        var response = await DoPut(METHOD, request, _token);

        response.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Name_Empty(string culture)
    {
        RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = await DoPut(METHOD, request, _token, culture);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);
        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(culture));
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectMessage));
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Email_Already_Register(string culture)
    {
        RequestUpdateUserJson request = RequestUpdateUserJsonBuilder.Build();
        request.Email = _emailAdmin;

        var result = await DoPut(METHOD, request, _token, culture);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);
        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_ALREADY_REGISTER", new CultureInfo(culture));
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectMessage));
    }

}
