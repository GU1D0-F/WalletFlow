using WalletFlow.Domain.Entities.Users;
using WalletFlow.Shared.Dtos;

namespace WalletFlow.Application.Users.Adapters;

public static class UserMap
{
    public static UserDto Map(User user) => new(user.Id.ToString(), user.FirstName, user.LastName, user.Email!, user.Wallet.Id.ToString());
}