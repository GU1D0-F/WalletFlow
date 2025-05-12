using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WalletFlow.Application.Wallets.Commands.AddFunds;
using WalletFlow.Application.Wallets.Commands.CreateTransfer;
using WalletFlow.Application.Wallets.Queries.GetUserTransfers;
using WalletFlow.Application.Wallets.Queries.GetWalletBalance;
using WalletFlow.Shared.Dtos;
using WalletFlow.Shared.Models.Wallets.Requests;
using WalletFlow.Shared.Wrappers;

namespace WalletFlow.Api.Controllers;

[ApiController]
[Route("api/v1/wallet")]
[Authorize]
public class WalletController(IMediator mediator) : ControllerBase
{
    [HttpGet("balance")]
    public async Task<ActionResult<ApiResponse<decimal>>> GetBalance()
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        var result = await mediator.Send(new GetWalletBalanceQuery(userId));

        return Ok(new ApiResponse<decimal>(result));
    }

    [HttpPost("add-funds")]
    public async Task<ActionResult<ApiResponse<string>>> AddFunds([FromBody] AddFundsRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        await mediator.Send(new AddFundsCommand(userId, request.Amount));

        return Ok(new ApiResponse<string>("Funds added successfully"));
    }
    
    [HttpPost("transfer")]
    public async Task<ActionResult<ApiResponse<WalletEntryDto>>> Transfer([FromBody] TransferRequest request)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var entryDto = await mediator.Send(new CreateTransferCommand(
            userId,
            request.ToWalletId,
            request.Amount,
            request.Description
        ));
        return Ok(new ApiResponse<WalletEntryDto>(entryDto));
    }

    [HttpGet("transfers")]
    public async Task<ActionResult<ApiResponse<List<WalletEntryDto>>>> GetTransfers(
        [FromQuery] DateTime? from = null,
        [FromQuery] DateTime? to   = null)
    {
        var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var transfers = await mediator.Send(new GetUserTransfersQuery(userId, from, to));
        return Ok(new ApiResponse<List<WalletEntryDto>>(transfers));
    }
}