using CashFlow.Communication.Request;
using CashFlow.Exception;
using CommomTestUtilities.Request;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Users.Register;
public class RegisterUserTest : CashFlowClassFixture
{
    private const string METHOD = "api/Users";

    public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
    }

    [Fact]
    public async Task Sucess()
    {
        RequestUserJson request = RequestRegisterUserJsonBuilder.Build();

        var result = await DoPost(METHOD, request);

        result.StatusCode.Should().Be(HttpStatusCode.Created);

        //var response = await result.Content.ReadFromJsonAsync<ResponseRegisterUserJson>() - Não é recomendado fazer desta forma, porque o usuário, no teste, não saberá qual é o formato de retorno, portnato, o ideal não é desserializar o retorno

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);
        // GetProperty - Porque é um documento json.
        response.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
        response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty();
    }
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))] // Forma de fazer com que seja criado uma lista de valores;
    public async Task Error_Empty_Name(string cultureInfo)
    {
        RequestUserJson request = RequestRegisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var result = await DoPost(requestUri: METHOD, request: request, cultureInfo: cultureInfo);


        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

         var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);
        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(cultureInfo));
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectMessage));
    }
}
