using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommomTestUtilities.Repositories.User;
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

    public UserReadOnlyRepositoryBuilder GetUserByEmail(CashFlow.Domain.Entities.User user)
    {
        _repository.Setup(userRepo => userRepo.GetUserByEmail(user.Email)).ReturnsAsync(user);
        // Para retornar a instância e encadear
        return this;
    }

    public UserReadOnlyRepositoryBuilder GetById(CashFlow.Domain.Entities.User user)
    {
        _repository.Setup(userRepo => userRepo.GetById(user.Id)).ReturnsAsync(user);

        return this;
    }


    public IUserReadOnlyRepository Build() => _repository.Object;
}
