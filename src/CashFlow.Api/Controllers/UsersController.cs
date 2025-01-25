using CashFlow.Application.UseCase.Users.ChangePassword;
using CashFlow.Application.UseCase.Users.Delete;
using CashFlow.Application.UseCase.Users.GetProfile;
using CashFlow.Application.UseCase.Users.Register;
using CashFlow.Application.UseCase.Users.Update;
using CashFlow.Communication.Request;
using CashFlow.Communication.Responses;
using CashFlow.Communication.Responses.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers;
[Route("api/[controller]")]
[ApiController]

public class UsersController : ControllerBase
{
    [HttpPost]
    [ProducesResponseType(typeof(ResponseRegisterUserJson), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Register(
        [FromServices] IRegisterUserUseCase _useCase,
        [FromBody] RequestRegisterUserJson request)
    {
        ResponseRegisterUserJson response = await _useCase.Execute(request);

        return Created(string.Empty, response);
    }

    [HttpGet]
    [Authorize]
    [ProducesResponseType(typeof(ResponseUserProfileJson), StatusCodes.Status200OK)]
    public async Task<IActionResult> Get([FromServices] IGetUserProfileUseCase useCase)
    {
        ResponseUserProfileJson response = await useCase.Execute();

        return Ok(response);
    }

    [HttpPut]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update([FromServices] IUpdateUserUseCase useCase, [FromBody] RequestUpdateUserJson request)
    {
        await useCase.Execute(request);

        return NoContent();
    }

    [HttpDelete]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete([FromServices] IDeleteUserUseCase useCase)
    {
        await useCase.Execute();

        return NoContent();
    }

    [HttpPut("change-password")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> ChangePassword(
        [FromServices] IChangePasswordUserUseCase useCase,
        [FromBody] RequestChangePasswordJson request)
    {
        await useCase.Execute(request);

        return NoContent();
    }
}
