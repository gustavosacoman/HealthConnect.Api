using HealthConnect.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Domain.Models.Specialities;

public class DoctorSpeciality : IAuditable, ISoftDeletable
{
    public Guid DoctorId { get; set; }

    public Doctor Doctor { get; set; }

    public Guid SpecialityId { get; set; }

    public Speciality Speciality { get; set; }

    required public string RqeNumber { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
