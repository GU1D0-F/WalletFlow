using MediatR;
using WalletFlow.Shared.Dtos;

namespace WalletFlow.Application.Users.Queries.GetProfile;

public record GetProfileQuery : IRequest<UserDto>;