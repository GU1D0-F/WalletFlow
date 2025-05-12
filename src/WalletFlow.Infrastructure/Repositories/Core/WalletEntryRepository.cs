using WalletFlow.Application.Repositories.Core;
using WalletFlow.Domain.Entities.Wallets;

namespace WalletFlow.Infrastructure.Repositories.Core;

public class WalletEntryRepository(CoreDbContext context) : BaseRepository<WalletEntry>(context), IWalletEntryRepository
{
}