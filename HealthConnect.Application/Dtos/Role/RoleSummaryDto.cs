using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Application.Dtos.Role;

public record RoleSummaryDto
{
    public Guid Id { get; init; }
    public string Name { get; init; }

}
