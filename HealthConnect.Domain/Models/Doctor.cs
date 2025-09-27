namespace HealthConnect.Domain.Models;

using HealthConnect.Domain.Interfaces;
using HealthConnect.Domain.Models.Specialities;

public class Doctor: IAuditable, ISoftDeletable
{
    public Guid Id { get; set; }

    required public string RQE { get; set; }

    public string? Biography { get; set; }

    public ICollection<DoctorSpeciality> DoctorSpecialities { get; set; }

    //public string ProfilePicture { get; set; }

    required public virtual User User { get; set; }

    public ICollection<Availability>? Availabilities { get; set; }

    public ICollection<DoctorCRM> DoctorCRMs { get; set; }

    required public Guid UserId { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
