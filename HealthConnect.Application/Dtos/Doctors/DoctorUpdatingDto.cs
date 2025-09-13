using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Application.Dtos.Doctors;

public record DoctorUpdatingDto
{
    public string? RQE { get; init; }

    public string? CRM { get; init; }

    public Guid SpecialityId { get; init; }

    public string? Biography { get; init; }
    //public string? ProfilePicture { get; init; }
}
