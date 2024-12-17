using CashFlow.Communication.Responses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionBase;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CashFlow.Api.Filters;
// Interface é um contrato, não há métodos definidos, há apenas os métodos que DEVEM ser definidos
public class ExceptionFilter : IExceptionFilter
{
    public void OnException(ExceptionContext context)
    {
        if(context.Exception is CashFlowException) // Deixo mais genérico o error. Porque eu poderia passar o ErrorOnValidationException, porém, este herda do Cashflow
        {
            HandleProjectException(context);
        }
        else
        {
            ThrowUnkowError(context);
        }
    }

    private void HandleProjectException(ExceptionContext context)
    {
        if(context.Exception is ErrorOnValidationException)
        {
            ErrorOnValidationException ex = (ErrorOnValidationException)context.Exception; // Ou  context.Exception as ErrorOnValidationException - Dessa forma se o ex não for o valor não ocorrerá o erro, irá retornar nulo
            var errorResponse = new ResponseErrorJson(ex.Errors);
            context.HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Result = new BadRequestObjectResult(errorResponse);
        }
    }

    private void ThrowUnkowError(ExceptionContext context)
    {
        var errorResponse = new ResponseErrorJson(ResourceErrorMessages.UNKNOWN_ERROR); // Forma para retornar um JSON
        context.HttpContext.Response.StatusCode = StatusCodes.Status500InternalServerError; // Forma para retornar o status codes 
        context.Result = new ObjectResult(errorResponse);
    }
}
