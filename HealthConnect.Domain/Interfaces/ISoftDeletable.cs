using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Domain.Interfaces;

public interface ISoftDeletable
{
    public DateTime? DeletedAt { get; set; }
}
