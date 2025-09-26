using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Application.Dtos.Doctors;

public record DoctorCrmDetailDto
{
    public string CrmNumber { get; init; }
    public string State { get; init; }
}
