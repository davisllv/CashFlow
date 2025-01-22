using CashFlow.Communication.Request;
using CashFlow.Exception;
using CommomTestUtilities.Request;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.Register;
public class RegisterExpenseTest : CashFlowClassFixture
{
    private const string METHOD = "api/Expenses";
    private readonly string _token;

    public RegisterExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.GetToken();
    }

    [Fact]
    public async Task Sucess()
    {
        RequestExpenseJson request = RequestRegisterExpenseJsonBuilder.Build();
        // Forma de perpassar o authorization
        var result = await DoPost(requestUri: METHOD, request: request, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.Created);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);
        // GetProperty - Porque é um documento json.
        response.RootElement.GetProperty("title").GetString().Should().Be(request.Title);
    }

    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Title_Empty(string cultureInfo)
    {
        RequestExpenseJson request = RequestRegisterExpenseJsonBuilder.Build();
        request.Title = string.Empty;

        // Esses códigos estão muito repetidos, por enquanto em todos os códigos de testes de integração
        // Criado esse formato, pois a extensão da classe vai fazer essa funcionalidade.
        var result = await DoPost(requestUri: METHOD, request: request, token: _token, cultureInfo: cultureInfo);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);
        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(cultureInfo));
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectMessage));

    }
}
