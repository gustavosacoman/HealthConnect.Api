using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Application.Dtos.Doctors;

public record DoctorSummaryDto
{
    required public Guid Id { get; init; }

    public string Name { get; init; }

    public string? Biography { get; init; }

    public IReadOnlyCollection<DoctorSpecialityDetailDto> Specialities { get; init; }

    public IReadOnlyCollection<DoctorCrmDetailDto> CRMs { get; init; }

    public IReadOnlyCollection<string> Roles { get; init; }

    //public string ProfilePicture { get; init; }
}
