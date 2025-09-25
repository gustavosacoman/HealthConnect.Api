using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Application.Dtos.DoctorCRM;

public record DoctorCRMRegistrationDto
{
    public Guid DoctorId { get; set; }
    public string CRMNumber { get; set; }
    public string State { get; set; }
}
