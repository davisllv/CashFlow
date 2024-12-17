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
    public IActionResult Register([FromBody] RequestRegisterExpenseJson request)
    {
            ResponseRegisterExpenseJson response = new RegisterExpenseUseCase().Execute(request);

            return Created();
    }
}
