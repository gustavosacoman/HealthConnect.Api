using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Domain.Interfaces;

public interface IAuditable
{
    DateTime CreateAt { get; set; }

    DateTime UpdateAt { get; set; }
}
