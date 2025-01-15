using CashFlow.Application.UseCase.Users.Register;
using CashFlow.Communication.Request;
using CashFlow.Communication.Responses.Users;
using CashFlow.Exception.ExceptionBase;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorOnValidationException), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase _useCase,
        [FromBody] RequestUserJson request)
    {
        ResponseRegisterUserJson response = await _useCase.Execute(request);

        return Created(string.Empty, response);
    }
}
