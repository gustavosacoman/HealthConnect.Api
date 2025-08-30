using HealthConnect.Domain.Interfaces;

namespace HealthConnect.Domain.Models;

public class Doctor: IAuditable, ISoftDeletable
{
    public Guid Id { get; set; }

    required public string RQE { get; set; }

    required public string CRM { get; set; }

    public string? Biography { get; set; }

    required public string Specialty { get; set; }

    //public string ProfilePicture { get; set; }

    required public virtual User User { get; set; }

    required public Guid UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
