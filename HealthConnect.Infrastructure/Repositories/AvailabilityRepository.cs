namespace HealthConnect.Infrastructure.Repositories;

using HealthConnect.Application.Dtos.Availability;
using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Domain.Models;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class AvailabilityRepository(AppDbContext appDbContext) : IAvailabilityRepository
{
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task CreateAvailabilityAsync(Availability availability)
    {
        await _appDbContext.Availabilities.AddAsync(availability);
    }

    public IQueryable<Availability> GetAllAvailabilityPerDoctor(Guid doctorId)
    {
        return _appDbContext.Availabilities
        .Where(a => a.DoctorId == doctorId);
    }

    public async Task<Availability> GetAvailabilityByIdAsync(Guid id)
    {
        return await _appDbContext.Availabilities.FindAsync(id);
    }
}
