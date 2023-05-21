using Application.Users.Commands;
using Application.Users.Queries.GetUserById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Presentation.Abstractions;

namespace Presentation.Controllers;

[Route("api/users")]
public sealed class UsersController : ApiController
{
    public UsersController(ISender sender) : base(sender)
    {
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(Guid id, CancellationToken cancellationToken)
    {
        var query = new GetUserByIdQuery(id);

        var response = await Sender.Send(query, cancellationToken);

        return response.IsSuccess ? Ok(response.Value) : NotFound(response.Error);
    }


    [HttpPost]
    public async Task<IActionResult> RegisterUser(string name, string phoneNumber, CancellationToken cancellationToken)
    {
        var query = new CreateUserCommand(name, phoneNumber);

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
}