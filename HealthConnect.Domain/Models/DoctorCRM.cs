using HealthConnect.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Domain.Models;

public class DoctorCRM : IAuditable, ISoftDeletable
{
    public Guid Id { get; set; }

    public Guid DoctorId { get; set; }

    public string CRMNumber { get; set; }

    public string State { get; set; }

    required public Doctor Doctor { get; set; }

    public DateTime CreatedAt { get; set; }

    public DateTime UpdatedAt { get; set; }

    public DateTime? DeletedAt { get; set; }
}
