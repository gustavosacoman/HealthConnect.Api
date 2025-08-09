using HealthConnect.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Application.Interfaces;

public interface IUserService
{
    public Task<UserSummaryDto> GetUserById(Guid id);

    public Task<UserSummaryDto> GetUserByEmail(string email);

    public Task<IEnumerable<UserSummaryDto>> GetAllUsers();

    public Task<UserSummaryDto> CreateUser(UserRegistrationDto data);

    public Task<UserSummaryDto> UpdateUser(Guid Id, UserUpdatingDto data);

    public Task DeleteUser(Guid Id);
}
