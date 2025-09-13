namespace HealthConnect.Domain.Models;

using HealthConnect.Domain.Interfaces;

public class Doctor: IAuditable, ISoftDeletable
{
    public Guid Id { get; set; }

    required public string RQE { get; set; }

    required public string CRM { get; set; }

    public string? Biography { get; set; }

    public Speciality Speciality { get; set; }

    public Guid SpecialityId { get; set; }

    //public string ProfilePicture { get; set; }

    required public virtual User User { get; set; }

    public ICollection<Availability>? Availabilities { get; set; }

    required public Guid UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
