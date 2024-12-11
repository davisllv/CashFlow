using CashFlow.Application.UseCase.Expenses.Register;
using CashFlow.Communication.Request;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{
    [HttpPost]

    public IActionResult Register([FromBody] RequestRegisterExpenseJson request)
    {
        try
        {
            ResponseRegisterExpenseJson response = new RegisterExpenseUseCase().Execute(request);

            return Created();
        }
        catch(ArgumentException ex) // Exception é uma forma mais genérica de erro.
        {
            var errorResponse = new ResponseErrorJson(ex.Message);
            return BadRequest(errorResponse);
        }
        //catch(DivideByZeroException ex)
        //{
        //    // Posso ter mais de um catch
        //}
        //catch (Exception ex) // Forma de definir uma excpection default.
        //{
        //}
        catch // Possivel passar dessa forma, ele torna-se default
        {
            var errorResponse = new ResponseErrorJson("unknown error"); // Forma para retornar um JSON
            
            return StatusCode(StatusCodes.Status500InternalServerError, errorResponse);

        }
        //finally
        //{
        //    // Vai cair nisso aqui independente se der erro ou não
        //}
    }
}
