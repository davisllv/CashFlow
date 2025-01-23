using CashFlow.Communication.Request;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCase.Users.Update;
public class UpdateUseValidator : AbstractValidator<RequestUpdateUserJson>
{

    public UpdateUseValidator()
    {
        RuleFor(user => user.Name).NotEmpty().WithMessage(ResourceErrorMessages.NAME_EMPTY);
        RuleFor(user => user.Email).NotEmpty().WithMessage(ResourceErrorMessages.EMAIL_EMPTY)
            .EmailAddress()
            .When(user => !string.IsNullOrWhiteSpace(user.Email), ApplyConditionTo.CurrentValidator)
            .WithMessage(ResourceErrorMessages.EMAIL_INVALID);
    }
}
