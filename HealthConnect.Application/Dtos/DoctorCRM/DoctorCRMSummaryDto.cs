using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Application.Dtos.DoctorCRM;

public record DoctorCRMSummaryDto
{
    public Guid Id { get; init; }

    public Guid DoctorId { get; init; }

    public string DoctorName { get; init; }

    public string CRMNumber { get; init; }

    public string State { get; init; }
}
