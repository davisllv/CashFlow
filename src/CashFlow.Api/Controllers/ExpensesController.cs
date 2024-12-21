using CashFlow.Application.UseCase.Expenses.Register;
using CashFlow.Communication.Request;
using CashFlow.Communication.Responses;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ExpensesController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> Register([FromServices] IRegisterExpenseUseCase useCase, [FromBody] RequestRegisterExpenseJson request)
    {
            ResponseRegisterExpenseJson response = await useCase.Execute(request);
            return Created();   
    }
}
