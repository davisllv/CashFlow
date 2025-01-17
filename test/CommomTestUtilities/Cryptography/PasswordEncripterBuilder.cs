using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Tokens;
using Moq;

namespace CommomTestUtilities.Cryptography;
public class PasswordEncripterBuilder
{
    public static IPasswordEncripter Build()
    {
        var mock = new Mock<IPasswordEncripter>();
        // Esse é o formato feito caso a função de dentro possua parâmetros, caso não haja eu não preciso passar o It.IsAny
        //mock.Setup(acess => acess.Encrypt()).Returns("eyJhbGciOiJIUzI1NiIsInR5cCI6Ik");
        mock.Setup(acess => acess.Encrypt(It.IsAny<string>())).Returns("eyJhbGciOiJIUzI1NiIsInR5cCI6Ik");

        return mock.Object;
    }
}
