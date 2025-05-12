namespace WalletFlow.Shared.Models.Users.Requests;

public class LoginRequestDto
{
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
}