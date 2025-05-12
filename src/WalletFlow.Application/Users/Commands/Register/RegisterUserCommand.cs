using MediatR;
using WalletFlow.Shared.Dtos;

namespace WalletFlow.Application.Users.Commands.Register;

public record RegisterUserCommand(string FirstName, string LastName, string Email, string Password) : IRequest<UserDto>;