using Bogus;
using CashFlow.Communication.Enums;
using CashFlow.Communication.Request;

namespace CommomTestUtilities.Requests;
// Esse é um arquivo para definir todas as requests de forma padrão para os testes. Isto é, não definir todas as vezes
public class RequestRegisterExpenseJsonBuilder
{
    public static RequestRegisterExpenseJson Build()
    {
        /*var faker = new Faker();

        var request = new RequestRegisterExpenseJson()
        {
           Title = faker.Commerce.Product(),
            Date = faker.Date.Past(),

        Um dos formatos para definir os dados}*/

        return new Faker<RequestRegisterExpenseJson>()
            .RuleFor(r => r.Title, faker => faker.Commerce.ProductName())
            .RuleFor(r => r.Description, faker => faker.Commerce.ProductDescription())
            .RuleFor(r => r.Date, faker => faker.Date.Past())
            .RuleFor(r => r.PaymentType, faker => faker.PickRandom<PaymentType>()) // Forma de definir os valores com enums, ele vai pegar o valor randomico dentro 
            .RuleFor(r => r.Amount, faker => faker.Random.Decimal(min: 1, max: 1000)); 
    }
}
