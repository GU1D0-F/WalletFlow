namespace WalletFlow.Shared.Models.Users.Responses;

public record LoginResultDto(string Token, DateTime ExpiresAt);