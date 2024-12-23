using CashFlow.Communication.Request;
using CashFlow.Exception;
using FluentValidation;

namespace CashFlow.Application.UseCase.Expenses;

public class ExpenseValidator : AbstractValidator<RequestExpenseJson>
{
    // Colocando essas mensagens eu quebro o compartilhamento dos dados. Crio um arquivo de Resource para facilitar a guarda dos dados, posso mudar em apenas um lugar.
    // Sempre que for utilizar um arquivo resource, eu preciso de um que não tenha nenhuma tag. Ele vai ser o default.
    // O arquivo de resource default ele vai retonar caso não seja encontrado um outro com a tag específico. O arquivo sem tag será definido o arquivo - idioma, nesse caso - padrão.
    public ExpenseValidator()
    {
        RuleFor(expense => expense.Title).NotEmpty().WithMessage(ResourceErrorMessages.TITLE_REQUIRED);
        RuleFor(expense => expense.Amount).GreaterThan(0).WithMessage(ResourceErrorMessages.AMOUNT_MUST_BE_GREATHER_THAN_ZERO);
        RuleFor(expense => expense.Date).LessThanOrEqualTo(DateTime.UtcNow).WithMessage(ResourceErrorMessages.EXPENSES_CANNOT_FOR_THE_FUTERE);
        RuleFor(expense => expense.PaymentType).IsInEnum().WithMessage(ResourceErrorMessages.PAYMENT_TYPE_INVALID);
    }
}
