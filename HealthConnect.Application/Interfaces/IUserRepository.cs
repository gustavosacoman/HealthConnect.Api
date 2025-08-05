using HealthConnect.Domain.Models;
namespace HealthConnect.Application.Interfaces;

public interface IUserRepository
{
    public Task<IEnumerable<User>> GetAllUsers();

    public Task<User?> GetUserById(Guid Id);

    public Task<User?> GetUserByEmail(string Email);

    public Task AddUser(User User);

}
