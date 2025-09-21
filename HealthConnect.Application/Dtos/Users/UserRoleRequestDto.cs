using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Application.Dtos.Users;

public class UserRoleRequestDto
{
    public string Email { get; set; }

    public string RoleName { get; set; }
}
