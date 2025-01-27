using Bogus;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enums;

namespace CommomTestUtilities.Entities;
public class ExpensesBuilder
{
    public static Expense Build(User user)
    {
        return new Faker<Expense>()
           .RuleFor(r => r.Id, _ => 1)
          .RuleFor(r => r.Title, faker => faker.Commerce.ProductName())
          .RuleFor(r => r.Description, faker => faker.Commerce.ProductDescription())
          .RuleFor(r => r.Date, faker => faker.Date.Past())
          .RuleFor(r => r.PaymentType, faker => faker.PickRandom<PaymentType>())
          .RuleFor(r => r.UserId, _ => user.Id)
          .RuleFor(r => r.Amount, faker => faker.Random.Decimal(min: 1, max: 1000))
          .RuleFor(r => r.Tags, faker => faker.Make(2, () => new CashFlow.Domain.Entities.Tag
          {
              Id = 1,
              Value = faker.PickRandom<CashFlow.Domain.Enums.Tag>(),
              ExpenseId = 1
          }));
    }

    public static List<Expense> Collection(User user)
    {
        uint currentId = 1;
        return new Faker<Expense>()
            .RuleFor(r => r.Id, _ => currentId++)
            .RuleFor(r => r.Title, faker => faker.Commerce.ProductName())
            .RuleFor(r => r.Description, faker => faker.Commerce.ProductDescription())
            .RuleFor(r => r.Date, faker => faker.Date.Past())
            .RuleFor(r => r.PaymentType, faker => faker.PickRandom<PaymentType>())
            .RuleFor(r => r.PaymentType, faker => faker.PickRandom<PaymentType>())
            .RuleFor(r => r.Amount, faker => faker.Random.Decimal(min: 1, max: 1000))
            .RuleFor(r => r.UserId, _ => user.Id)
            .Generate(5);

    }
}
