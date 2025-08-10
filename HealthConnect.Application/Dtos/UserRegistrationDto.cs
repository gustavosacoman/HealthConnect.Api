using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Application.Dtos;

public record UserRegistrationDto
{
    public string Name { get; init; }

    public string Email { get; init; }

    public string Phone { get; init; }

    public string Password { get; init; }

    public string CPF { get; init; }

    public DateOnly BirthDate { get; init; }
}
