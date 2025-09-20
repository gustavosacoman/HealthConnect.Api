using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Application.Dtos.Doctors;

public record DoctorSummaryDto
{
    required public Guid Id { get; init; }

    required public string RQE { get; init; }

    required public string CRM { get; init; }

    required public string Speciality { get; init; }

    public string Name { get; init; }

    public string? Biography { get; init; }

    public IReadOnlyCollection<string> DoctorRoles { get; init; }

    //public string ProfilePicture { get; init; }
}
