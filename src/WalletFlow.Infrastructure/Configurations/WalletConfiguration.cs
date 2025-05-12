using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletFlow.Domain.Entities.Wallets;

namespace WalletFlow.Infrastructure.Configurations;

public class WalletConfiguration: IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.ToTable("Wallets");

        builder.HasKey(w => w.Id);
        
        builder.Property(w => w.UserId)
            .IsRequired();
        
        builder.Property(w => w.Balance)
            .IsRequired()
            .HasColumnType("decimal(18,2)");
        
        builder.Property(w => w.Currency)
            .IsRequired()
            .HasMaxLength(3)
            .IsUnicode(false);
        
        builder.Property(w => w.CreatedAt)
            .IsRequired();
        
        builder.HasOne(w => w.User)
            .WithOne(u => u.Wallet)
            .HasForeignKey<Wallet>(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);

    }
}