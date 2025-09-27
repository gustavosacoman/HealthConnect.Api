namespace HealthConnect.Infrastructure.Repositories;

using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Domain.Models;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// Repository for managing doctors crms.
/// </summary>
public class DoctorCRMRepository(
    AppDbContext appDbContext)
    : IDoctorCRMRepository
{
    private readonly AppDbContext _appDbContext = appDbContext;

    /// <inheritdoc/>
    public async Task CreateCRMAsync(DoctorCRM doctorCRM)
    {
        await _appDbContext.DoctorCRMs.AddAsync(doctorCRM);
    }

    /// <inheritdoc/>
    public async Task<DoctorCRM?> GetByIdAsync(Guid id)
    {
        return await _appDbContext.DoctorCRMs
            .Include(c => c.Doctor)
            .ThenInclude(d => d.User)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    /// <inheritdoc/>
    public IQueryable<DoctorCRM> GetAllCRMAsync()
    {
        return _appDbContext.DoctorCRMs.AsNoTracking();
    }

    /// <inheritdoc/>
    public async Task<DoctorCRM?> GetCRMByCodeAndState(string code, string state)
    {
        return await _appDbContext
            .DoctorCRMs
            .Include(c => c.Doctor)
            .ThenInclude(d => d.User)
            .FirstOrDefaultAsync(c => c.CRMNumber == code && c.State == state);
    }
}
