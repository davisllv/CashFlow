using Bogus;
using CashFlow.Communication.Request;

namespace CommomTestUtilities.Request;
public class RequestDoLoginJsonBuilder
{
    public static RequestLoginJson Build()
    {
        return new Faker<RequestLoginJson>()
            .RuleFor(r => r.Email, faker => faker.Internet.Email())
            .RuleFor(r => r.Password, faker => faker.Internet.Password());
    }
}
