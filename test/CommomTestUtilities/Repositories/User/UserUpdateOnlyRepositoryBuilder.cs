using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommomTestUtilities.Repositories.User;
public class UserUpdateOnlyRepositoryBuilder
{
    public static IUserUpdateOnlyRepository Build()
    {
        var mock = new Mock<IUserUpdateOnlyRepository>();
        return mock.Object;
    }
}
