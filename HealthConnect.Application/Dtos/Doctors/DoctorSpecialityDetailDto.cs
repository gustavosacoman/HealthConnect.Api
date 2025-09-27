using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Application.Dtos.Doctors;

public record DoctorSpecialityDetailDto
{
    public string SpecialityName { get; init; }
    public string RqeNumber { get; init; }
}
