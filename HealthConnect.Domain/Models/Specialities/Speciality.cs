using HealthConnect.Domain.Interfaces;

namespace HealthConnect.Domain.Models.Specialities;

public class Speciality : IAuditable, ISoftDeletable
{

    public Guid Id { get; set; }

    required public string Name { get; set; }

    public ICollection<DoctorSpeciality> DoctorSpecialities { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
