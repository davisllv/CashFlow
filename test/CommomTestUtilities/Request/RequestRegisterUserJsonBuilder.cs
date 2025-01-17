using Bogus;
using CashFlow.Communication.Request;

namespace CommomTestUtilities.Request;
public class RequestRegisterUserJsonBuilder
{
    public static RequestUserJson Build()
    {
        return new Faker<RequestUserJson>()
            .RuleFor(user => user.Name, faker => faker.Person.FirstName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1"));
            
    }
}
