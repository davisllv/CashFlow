using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommomTestUtilities.Repositories;
public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;

    public UserReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUserReadOnlyRepository>();
    }
    public void ExistActiveUserWithEmail(string email)
    {
        // Forma de definir um formato diferente para retornar um valor true caso esse valor seja igual ao digitado
        _repository.Setup(userReadOnly => userReadOnly.ExistActiveUserWithEmail(email)).ReturnsAsync(true);
    }

    public IUserReadOnlyRepository Build() => _repository.Object;
}
