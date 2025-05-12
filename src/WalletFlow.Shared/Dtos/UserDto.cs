namespace WalletFlow.Shared.Dtos;

public record UserDto(
    string Id,
    string FirstName,
    string LastName,
    string Email,
    string WalletId
);