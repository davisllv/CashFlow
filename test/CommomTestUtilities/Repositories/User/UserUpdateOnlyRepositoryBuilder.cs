using CashFlow.Domain.Repositories.Users;
using Moq;

namespace CommomTestUtilities.Repositories.User;
public class UserUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IUserUpdateOnlyRepository> _repository;
    public UserUpdateOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUserUpdateOnlyRepository>();
    }

    public UserUpdateOnlyRepositoryBuilder GetById(CashFlow.Domain.Entities.User user)
    {
        _repository.Setup(userRepo => userRepo.GetById(user.Id)).ReturnsAsync(user);

        return this;
    }

    public IUserUpdateOnlyRepository Build() => _repository.Object;

}
