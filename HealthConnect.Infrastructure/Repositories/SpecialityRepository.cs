namespace HealthConnect.Infrastructure.Repositories;

using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Domain.Models.Specialities;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
public class SpecialityRepository(AppDbContext appDbContext) : ISpecialityRepository
{
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task<Speciality> GetSpecialityByIdAsync(Guid Id)
    {
        return await _appDbContext.Specialities.FindAsync(Id);
    }

    public async Task<IEnumerable<Speciality>> GetAllSpecialitiesAsync()
    {
        return await _appDbContext.Specialities.ToListAsync();
    }

    public async Task<Speciality> GetSpecialityByNameAsync(string name)
    {
        return await _appDbContext.Specialities.FirstOrDefaultAsync(s => s.Name == name);
    }

    public async Task<IEnumerable<Speciality>> GetSpecialitiesForDoctor(Guid doctorId)
    {
        return await _appDbContext.Specialities
            .Where(s => s.DoctorSpecialities
            .Any(ds => ds.DoctorId == doctorId))
            .ToListAsync();
    }

    public async Task CreateSpecialityAsync(Speciality speciality)
    {
        await _appDbContext.Specialities.AddAsync(speciality);
    }
}
