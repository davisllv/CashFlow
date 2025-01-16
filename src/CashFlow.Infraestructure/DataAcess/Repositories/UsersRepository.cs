using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Users;
using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infraestructure.DataAcess.Repositories;
internal class UsersRepository : IUserWriteOnlyRepository, IUserReadOnlyRepository
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

    public async Task<bool> ExistActiveUserWithEmail(string email)
    {
        bool hasUser = await _dbContext.User.AnyAsync(u => u.Email.Equals(email));

        return hasUser;
    }
}
