using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Domain.Models;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Infrastructure.Repositories;

public class DoctorCRMRepository(AppDbContext appDbContext) : IDoctorCRMRepository
{
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task CreateCRMAsync(DoctorCRM doctorCRM)
    {
        await _appDbContext.DoctorCRMs.AddAsync(doctorCRM);
    }

    public async Task<DoctorCRM?> GetByIdAsync(Guid id)
    {
        return await _appDbContext.DoctorCRMs
            .Include(c => c.Doctor)
            .ThenInclude(d => d.User)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public IQueryable<DoctorCRM> GetAllCRMAsync()
    {
        return _appDbContext.DoctorCRMs.AsNoTracking();
    }

    public async Task<DoctorCRM?> GetCRMByCodeAndState(string code, string state)
    {
        return await _appDbContext
            .DoctorCRMs
            .Include(c => c.Doctor)
            .ThenInclude(d => d.User)
            .FirstOrDefaultAsync(c => c.CRMNumber == code && c.State == state);
    }
}
