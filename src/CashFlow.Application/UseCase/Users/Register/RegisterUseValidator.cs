using CashFlow.Communication.Request;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCase.Users.Register;
public class UserUseValidator : AbstractValidator<RequestRegisterUserJson>
{
    public UserUseValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress()
            .When(user => !string.IsNullOrWhiteSpace(user.Email), ApplyConditionTo.CurrentValidator) // Forma de definir que só vai passar para a segunda validação se não for nulo
            .WithMessage(ResourceErrorMessages.EMAIL_INVALID);

        // FOrma de criar uma maneira de reutilizar a validação da senha
        RuleFor(user => user.Password).SetValidator(new PasswordValidator<RequestRegisterUserJson>());
    }
}
