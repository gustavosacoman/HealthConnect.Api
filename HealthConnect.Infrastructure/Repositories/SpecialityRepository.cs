namespace HealthConnect.Infrastructure.Repositories;

using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Domain.Models.Specialities;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Repository for managing Speciality entities.
/// </summary>
public class SpecialityRepository(
    AppDbContext appDbContext)
    : ISpecialityRepository
{
    private readonly AppDbContext _appDbContext = appDbContext;

    /// <inheritdoc />
    public async Task<Speciality?> GetSpecialityByIdAsync(Guid id)
    {
        return await _appDbContext.Specialities.FindAsync(id);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Speciality>> GetAllSpecialitiesAsync()
    {
        return await _appDbContext.Specialities.ToListAsync();
    }

    /// <inheritdoc />
    public async Task<Speciality?> GetSpecialityByNameAsync(string name)
    {
        return await _appDbContext.Specialities.FirstOrDefaultAsync(s => s.Name == name);
    }

    /// <inheritdoc />
    public async Task<IEnumerable<Speciality>> GetSpecialitiesForDoctor(Guid doctorId)
    {
        return await _appDbContext.Specialities
            .Where(s => s.DoctorSpecialities
            .Any(ds => ds.DoctorId == doctorId))
            .ToListAsync();
    }

    /// <inheritdoc />
    public async Task<DoctorSpeciality?> GetDoctorSpecialityByRqe(string rqeNumber)
    {
        return await _appDbContext.DoctorSpecialities
            .Include(ds => ds.Doctor)
            .Include(ds => ds.Speciality)
            .FirstOrDefaultAsync(ds => ds.Doctor.RQE == rqeNumber);
    }

    /// <inheritdoc />
    public async Task CreateSpecialityAsync(Speciality speciality)
    {
        await _appDbContext.Specialities.AddAsync(speciality);
    }
}
