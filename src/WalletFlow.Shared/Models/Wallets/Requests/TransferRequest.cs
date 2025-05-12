using System.ComponentModel.DataAnnotations;

namespace WalletFlow.Shared.Models.Wallets.Requests;

public class TransferRequest
{
    [Required]
    public Guid ToWalletId { get; set; }

    [Required]
    public decimal Amount { get; set; }

    public string? Description { get; set; }
}