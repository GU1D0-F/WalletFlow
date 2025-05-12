using WalletFlow.Application.Repositories.Core;
using WalletFlow.Domain.Entities.Wallets;

namespace WalletFlow.Infrastructure.Repositories.Core;

public class WalletRepository(CoreDbContext context) : BaseRepository<Wallet>(context), IWalletRepository
{
    
}