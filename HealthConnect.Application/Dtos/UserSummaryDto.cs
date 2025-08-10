using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Application.Dtos;

public record UserSummaryDto
{
    public Guid Id { get; }

    public string Name { get; }

    public string Email { get; }

    public string Phone { get; }

    public string CPF { get; }

    public DateOnly BirthDate { get; }

}
