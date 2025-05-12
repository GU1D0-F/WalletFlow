namespace WalletFlow.Domain.Entities;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; protected init; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; private set; }

    public void SetUpdatedAt()
    {
        UpdatedAt = DateTime.UtcNow;
    }
}