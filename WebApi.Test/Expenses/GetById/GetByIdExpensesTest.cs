using CashFlow.Communication.Enums;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InlineData;

namespace WebApi.Test.Expenses.GetById;
public class GetByIdExpensesTest : CashFlowClassFixture
{
    private const string METHOD = "api/Expenses";
    private readonly string _token;
    private readonly long _expenseId;
    public GetByIdExpensesTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_Team_Member.GetToken();
        _expenseId = webApplicationFactory.Expense.GetExpenseId();
    }

    [Fact]
    public async Task Sucess()
    {
        var result = await DoGet($"{METHOD}/{_expenseId}", _token);


        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);
        var id = response.RootElement.GetProperty("id").GetInt64();
        response.RootElement.GetProperty("id").GetInt64().Should().Be(_expenseId);
        response.RootElement.GetProperty("title").GetString().Should().NotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("date").GetDateTime().Should().NotBeAfter(DateTime.Today);
        response.RootElement.GetProperty("amount").GetDecimal().Should().BeGreaterThan(0);
        var paymentType = response.RootElement.GetProperty("paymentType").GetInt32();

        Enum.IsDefined(typeof(PaymentType), paymentType).Should().BeTrue();

    }
    [Theory]
    [ClassData(typeof(CultureInlineDataTest))]
    public async Task Error_Expense_Not_Found(string cultureInfo)
    {
        var result = await DoGet($"{METHOD}/{2000}", _token, cultureInfo: cultureInfo);


        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);
        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(cultureInfo));
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectMessage));
    }
}
