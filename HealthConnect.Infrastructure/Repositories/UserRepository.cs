using HealthConnect.Application.Interfaces;
using HealthConnect.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _appDbContext;

    public UserRepository(AppDbContext appDbContext)
    {
        _appDbContext = appDbContext;
    }

    public async Task CreateUser(User User)
    {
        await _appDbContext.Users.AddAsync(User);
    }

    public async Task<IEnumerable<User>> GetAllUsers()
    {
        return await _appDbContext.Users.ToListAsync();
    }

    public async Task<User?> GetUserByEmail(string Email)
    {
        return await _appDbContext.Users.FirstOrDefaultAsync(u => u.Email == Email);
    }

    public async Task<User?> GetUserById(Guid Id)
    {
        return await _appDbContext.Users.FindAsync(Id);
    }
}
