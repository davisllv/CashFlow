using CashFlow.Domain.Entities;
using CashFlow.Domain.Security.Tokens;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Infrastructure.DataAccess;
using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace CashFlow.Infraestructure.Services.LoggedUser;
public class LoggedUser : ILoggedUser
{
    private readonly CashFlowDbContext _dbContext;
    private readonly ITokenProvider _tokenProvider;

    public LoggedUser(CashFlowDbContext dbContext, ITokenProvider tokenProvider)
    {
        _dbContext = dbContext;
        _tokenProvider = tokenProvider;
    }

    public async Task<User> Get()
    {
        // JwtSecurityTokenHandler - Para ler o Token;
        var jwtSecurityToken = new JwtSecurityTokenHandler().ReadJwtToken(_tokenProvider.TokenOnRequest());

        var identifier = jwtSecurityToken.Claims.First( claim => claim.Type == ClaimTypes.Sid).Value;

        return await _dbContext.Users
            .AsNoTracking()
            .FirstAsync(user => user.UserIdentifier == Guid.Parse(identifier));
    }
}
