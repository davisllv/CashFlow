using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CommomTestUtilities.Cryptography;
public class PasswordEncripterBuilder
{
    private readonly Mock<IPasswordEncripter> _mock;
    public PasswordEncripterBuilder()
    {

        _mock = new Mock<IPasswordEncripter>();
        // Esse é o formato feito caso a função de dentro possua parâmetros, caso não haja eu não preciso passar o It.IsAny
        //mock.Setup(acess => acess.Encrypt()).Returns("eyJhbGciOiJIUzI1NiIsInR5cCI6Ik");
        _mock.Setup(acess => acess.Encrypt(It.IsAny<string>())).Returns("eyJhbGciOiJIUzI1NiIsInR5cCI6Ik");
    }

    public PasswordEncripterBuilder Verify(string? password)
    {
        if(!string.IsNullOrWhiteSpace(password))
            _mock.Setup(acess => acess.Verify(password, It.IsAny<string>())).Returns(true);


        return this;
    }

    public IPasswordEncripter Build()
    {
         return _mock.Object;
    }
}
