using HealthConnect.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Domain.Models;

public class User : IAuditable
{
    public Guid Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string CPF { get; set; }

    public string HashedPassword { get; set; }

    public string Salt { get; set; }

    public DateOnly BirthDate { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }
}
