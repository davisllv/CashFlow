using CashFlow.Domain.Enums;

namespace CashFlow.Domain.Entities;
public class User
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public Guid UserIdentifier { get; set; } // Para criar o token, não utilizar o email, visto que pode causar incongruências ao alterar o email.
    public string Role { get; set; } = = Roles.TEAM_MEMBER;
}

