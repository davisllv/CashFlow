using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommomTestUtilities.Repositories.User;
public class UserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Build()
    {
        var mock = new Mock<IUserWriteOnlyRepository>();

        return mock.Object;
    }
}
