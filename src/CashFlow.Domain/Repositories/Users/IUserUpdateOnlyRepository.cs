using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Users;
public interface IUserUpdateOnlyRepository
{
    void Update(User user);
}
