using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WalletFlow.Domain.Entities.Wallets;

namespace WalletFlow.Infrastructure.Configurations;

public class WalletEntryConfiguration : IEntityTypeConfiguration<WalletEntry>
{
    public void Configure(EntityTypeBuilder<WalletEntry> builder)
    {
        // Nome da tabela
        builder.ToTable("WalletEntries");

        // Chave primária
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id)
            .ValueGeneratedNever();

        // Propriedades
        builder.Property(e => e.WalletId)
            .IsRequired();

        builder.Property(e => e.Type)
            .IsRequired();

        builder.Property(e => e.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.Description)
            .HasMaxLength(500);

        builder.Property(e => e.ReferenceId);

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        // Índices para melhorar performance de consultas
        builder.HasIndex(e => e.WalletId);
        builder.HasIndex(e => new { e.WalletId, e.CreatedAt });
        builder.HasIndex(e => e.ReferenceId);
    }
}