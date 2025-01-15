using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Infrastructure.DataAccess;

namespace CashFlow.Infraestructure.DataAcess.Repositories;
internal class UsersRepository : IUserWriteOnlyRepository
{
    private readonly CashFlowDbContext _dbContext;

    public UsersRepository(CashFlowDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Add(User user)
    {
        await _dbContext.User.AddAsync(user);
    }
}
