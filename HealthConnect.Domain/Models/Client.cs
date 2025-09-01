using HealthConnect.Domain.Interfaces;

namespace HealthConnect.Domain.Models;

public class Client : IAuditable, ISoftDeletable
{
    required public Guid Id { get; set; }

    required public Guid UserId { get; set; }

    required public virtual User User { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
