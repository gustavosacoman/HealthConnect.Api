namespace HealthConnect.Infrastructure.Repositories;

using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Domain.Models;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Repository for managing Doctor Offices.
/// </summary>
public class DoctorOfficeRepository(AppDbContext appDbContext)
    : IDoctorOfficeRepository
{
    private readonly AppDbContext _appDbContext = appDbContext;

    /// <inheritdoc/>
    public async Task<DoctorOffice?> GetDoctorOfficeByIdAsync(Guid id)
    {
        return await _appDbContext.DoctorOffices.FindAsync(id);
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<DoctorOffice>> GetAllDoctorOfficesAsync()
    {
        return await _appDbContext.DoctorOffices.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<IEnumerable<DoctorOffice>> GetOfficeByDoctorIdAsync(Guid doctorId)
    {
        return await _appDbContext.DoctorOffices
            .Where(of => of.DoctorId == doctorId)
            .ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<DoctorOffice?> GetPrimaryOfficeByDoctorIdAsync(Guid doctorId)
    {
        return await _appDbContext.DoctorOffices
            .Where(of => of.DoctorId == doctorId && of.IsPrimary == true)
            .FirstOrDefaultAsync();
    }

    /// <inheritdoc/>
    public async Task CreateDoctorOfficeAsync(DoctorOffice doctorOffice)
    {
        await _appDbContext.DoctorOffices.AddAsync(doctorOffice);
    }
}
