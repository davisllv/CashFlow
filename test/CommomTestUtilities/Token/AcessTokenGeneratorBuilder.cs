using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using Moq;

namespace CommomTestUtilities.Token;
public class AcessTokenGeneratorBuilder
{
    public static IAcessTokenGenerator Build()
    {
        var mock = new Mock<IAcessTokenGenerator>();

        mock.Setup(acess => acess.Generate(It.IsAny<User>())).Returns("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6IkRhdmkgZGEgU2lsdmEgZG9zIFNhbnRvcyIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL3NpZCI6ImM1YmRiMmU3LTYwNDQtNGFiMS1iZmUxLTg0M2Q2MTI0ZWI1OCIsIm5iZiI6MTczNzEzMTUxOCwiZXhwIjoxNzM3MTkxNTE4LCJpYXQiOjE3MzcxMzE1MTh9.dhaYnteY-QHKJ8gIAOj6g9cQb7JytXL78OBTFfwq-9I");

        return mock.Object;
    }
}
