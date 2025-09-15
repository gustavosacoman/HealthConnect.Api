namespace HealthConnect.Infrastructure.Repositories;

using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Domain.Models;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
public class SpecialityRepository(AppDbContext appDbContext) : ISpecialityRepository
{
    private readonly AppDbContext _appDbContext1 = appDbContext;

    public async Task<Speciality> GetSpecialityByIdAsync(Guid Id)
    {
        return await _appDbContext1.Specialities.FindAsync(Id);
    }

    public async Task<IEnumerable<Speciality>> GetAllSpecialitiesAsync()
    {
        return await _appDbContext1.Specialities.ToListAsync();
    }
    public async Task<Speciality> GetSpecialityByNameAsync(string name)
    {
        return await _appDbContext1.Specialities.FirstOrDefaultAsync(s => s.Name == name);
    }

    public async Task CreateSpecialityAsync(Speciality speciality)
    {
        await _appDbContext1.Specialities.AddAsync(speciality);
    }
}
