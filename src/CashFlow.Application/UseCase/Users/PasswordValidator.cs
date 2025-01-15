using CashFlow.Exception;
using FluentValidation;
using FluentValidation.Validators;
using System.Text.RegularExpressions;

namespace CashFlow.Application.UseCase.Users;
// T é um parâmetro dinamico ou genérico
public partial class PasswordValidator<T> : PropertyValidator<T, string>
{
    private const string ERROR_MESSAGE_KEY = "ErrorMessage";
    // Nome da função
    public override string Name => "PasswordValidator";

    protected override string GetDefaultMessageTemplate(string errorCode)
    {
        // Scape Caracter - Escapar o caracter especial.
        return $"{{{ERROR_MESSAGE_KEY}}}";
    }

    public override bool IsValid(ValidationContext<T> context, string password)
    {
        if (string.IsNullOrEmpty(password))
        {
            // AppendArgument - Aceita um chave:Valor; Nome da mensagem e a Mensagem de erro
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceErrorMessages.INVALID_PASSWORD);
            return  false;
        };

        if(password.Length < 8)
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceErrorMessages.INVALID_PASSWORD);
            return false;
        }
        // [A-Z] - Pelo menos um maiculo

        // [A-Z] - Pelo menos um MINUSCULO
        if (UpperCaseLetter().IsMatch(password) == false || LowerCaseLetter().IsMatch(password) == false || Numbers().IsMatch(password) == false || SpecialCaracteres().IsMatch(password) == false)
        {
            context.MessageFormatter.AppendArgument(ERROR_MESSAGE_KEY, ResourceErrorMessages.INVALID_PASSWORD);
            return false;
        }

        return true;
    }

    [GeneratedRegex(@"[A-Z]+")]
    private static partial Regex UpperCaseLetter();
    [GeneratedRegex(@"[a-z]+")]
    private static partial Regex LowerCaseLetter();
    [GeneratedRegex(@"[0-9]+")]
    private static partial Regex Numbers();
    [GeneratedRegex(@"[\!\?\*\.]+")]
    private static partial Regex SpecialCaracteres();
}
