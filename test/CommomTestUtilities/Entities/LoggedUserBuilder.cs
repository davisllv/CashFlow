using CashFlow.Domain.Services.LoggedUser;
using Moq;

namespace CommomTestUtilities.Entities;
public class LoggedUserBuilder
{
    private readonly Mock<ILoggedUser> _loggedUserMock;

    public LoggedUserBuilder()
    {
        _loggedUserMock = new Mock<ILoggedUser>();
    }

    public LoggedUserBuilder Get(CashFlow.Domain.Entities.User user)
    {
        _loggedUserMock.Setup(setup => setup.Get()).ReturnsAsync(user);

        return this;
    }

    public ILoggedUser Build() => _loggedUserMock.Object;
}
