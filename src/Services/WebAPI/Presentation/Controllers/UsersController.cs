using Application.Users.Command.ChangePassword;
using Application.Users.Command.CreateUser;
using Application.Users.Command.SetPassword;
using Application.Users.Login;
using Application.Users.Queries.GetUserById;
using Domain.Enums;
using Infrastructure.Authentication;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;
using Presentation.RequestParams.User;

namespace Presentation.Controllers;

[Route("api/[controller]/[action]")]
public sealed class UsersController : ApiController
{
    public UsersController(ISender sender) : base(sender)
    {
    }

    [HasPermission(Permission.ReadUser)]
    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(
        Guid id,
        CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }
    
    [HttpPost]
    public async Task<IActionResult> Register(
        [FromBody]RegisterUserRequest request,
        CancellationToken cancellationToken)
    {
        var query = new CreateUserCommand(request.Name, request.PhoneNumber, request.Email);

        var result = await Sender.Send(query,cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return CreatedAtAction(
            nameof(GetUserById),
            new {id = result.Value},
            result.Value);
    }

    [HttpPost]
    public async Task<IActionResult> Login(
        [FromBody]LoginRequest loginRequest, 
        CancellationToken cancellationToken)
    {
        var query = new LoginCommand(loginRequest.Email, loginRequest.Password);

        var result = await Sender.Send(query, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return Ok(result.Value);
    }
    
    [HttpPut("{id}"),Authorize]
    public async Task<IActionResult> SetPassword(
        Guid id,
        [FromBody] ResetPasswordRequest request,
        CancellationToken cancellationToken)
    {
        var command = new SetUserPasswordCommand(id, request.Password);

        var result = await Sender.Send(command,cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }

    [HttpPut("{id}"),Authorize]
    public async Task<IActionResult> ChangePassword(
        Guid id,
        [FromBody]ChangePasswordRequest request,
        CancellationToken cancellationToken)
    {
        var command = new ChangeUserPasswordCommand(id, request.OldPassword, request.NewPassword);

        var result = await Sender.Send(command, cancellationToken);

        if (result.IsFailure)
        {
            return HandleFailure(result);
        }

        return NoContent();
    }
}