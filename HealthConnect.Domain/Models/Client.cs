namespace HealthConnect.Domain.Models;

public class Client
{
    required public Guid Id { get; set; }

    required public Guid UserId { get; set; }

    required public virtual User User { get; set; }
}
