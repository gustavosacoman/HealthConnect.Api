using HealthConnect.Application.Dtos.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Application.Interfaces.ServicesInterface;

public interface IAuthService
{
    public Task<LoginResponseDto> LoginAsync(LoginRequestDto request);

}
