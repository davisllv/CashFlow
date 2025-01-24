using CashFlow.Communication.Request;
using FluentValidation;

namespace CashFlow.Application.UseCase.Users.ChangePassword;
public class ChangePasswordUserValidator : AbstractValidator<RequestChangePasswordJson>
{
    public ChangePasswordUserValidator()
    {
        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestChangePasswordJson>());
    }
}
