using FluentValidation;
using FluentValidation.Validators;

namespace CashFlow.Application.UseCase.Users;
// T é um parâmetro dinamico ou genérico
public class PasswordValidator<T> : PropertyValidator<T, string>
{
    // Nome da função
    public override string Name => "PasswordValidator";

    public override bool IsValid(ValidationContext<T> context, string password)
    {
        throw new NotImplementedException();
    }
}
