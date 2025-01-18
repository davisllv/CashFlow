using Bogus;
using CashFlow.Domain.Entities;
using CommomTestUtilities.Cryptography;

namespace CommomTestUtilities.Entities;
public class UserBuilder
{
    public static User Build()
    {
        var passwordEncripter = new PasswordEncripterBuilder().Build();

        var user = new Faker<User>()
            .RuleFor(u => u.Id, _ => 1)
            .RuleFor(u => u.Name, faker => faker.Person.FirstName)
            .RuleFor(u => u.Email, faker => faker.Internet.Email())
            .RuleFor(u => u.Password, (_, user) => passwordEncripter.Encrypt(user.Password))
            .RuleFor(u => u.UserIdentifier, _ => Guid.NewGuid());

        return user;
    }
}
