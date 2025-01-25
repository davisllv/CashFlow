using Bogus;
using CashFlow.Communication.Request;

namespace CommomTestUtilities.Request;
public class RequestChangePasswordJsonBuilder
{
    public static RequestChangePasswordJson Build()
    {
        return new Faker<RequestChangePasswordJson>()
            .RuleFor(user => user.Password, faker => faker.Internet.Password())
            .RuleFor(user => user.NewPassword, faker => faker.Internet.Password(prefix: "!Aa1"));
    }
}
