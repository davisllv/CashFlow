using CashFlow.Domain.Repositories;
using Moq;

namespace CommomTestUtilities.Repositories;
public class UnitOfWorkBuilder
{
    public static IUnitOfWork Build()
    {
        var mock = new Mock<IUnitOfWork>();

        return mock.Object; // Implementação fake da interface
    }
}
