using MediatR;
using WalletFlow.Shared.Models.Users.Responses;

namespace WalletFlow.Application.Users.Commands.Login;

public record LoginCommand(string Email, string Password) : IRequest<LoginResultDto>;