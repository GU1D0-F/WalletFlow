using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletFlow.Application.Users.Commands.Login;
using WalletFlow.Application.Users.Commands.Register;
using WalletFlow.Application.Users.Queries.GetProfile;
using WalletFlow.Shared.Dtos;
using WalletFlow.Shared.Models.Users.Requests;
using WalletFlow.Shared.Models.Users.Responses;
using WalletFlow.Shared.Wrappers;

namespace WalletFlow.Api.Controllers;

[ApiController]
[Route("api/v1/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    [HttpGet("profile"), Authorize]
    public async Task<ActionResult<ApiResponse<UserDto>>> GetProfile()
    {
        var result = await mediator.Send(new GetProfileQuery());
        return Ok(new ApiResponse<UserDto>(result));
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResultDto>>> Login([FromBody] LoginRequestDto request)
    {
        var command = new LoginCommand(request.Email, request.Password);
        var result = await mediator.Send(command);
        return Ok(new ApiResponse<LoginResultDto>(result));
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<UserDto>>> Register([FromBody] RegisterUserDto request)
    {
        var command = new RegisterUserCommand(
            request.FirstName, 
            request.LastName, 
            request.Email.ToLower(), 
            request.Password
        );

        var result = await mediator.Send(command);
        
        return CreatedAtAction(nameof(GetProfile), new { }, new ApiResponse<UserDto>(result));
    }
}