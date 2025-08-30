namespace HealthConnect.Infrastructure.Repositories;

using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Domain.Models;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

public class DoctorRepository(AppDbContext appDbContext) : IDoctorRepository
{
    private readonly AppDbContext _appDbContext = appDbContext;

    public async Task CreateDoctor(Doctor doctor)
    {
        await _appDbContext.Doctors.AddAsync(doctor);
    }

    public async Task<IEnumerable<Doctor>> GetAllDoctors()
    {
        return await _appDbContext.Doctors.ToListAsync();
    }

    public async Task<Doctor?> GetDoctorById(Guid id)
    {
        return await _appDbContext.Doctors
            .Include(d => d.User)
            .FirstOrDefaultAsync(d => d.Id == id && d.User != null);
    }

    public async Task<Doctor?> GetDoctorByRQE(string rqe)
    {
        return await _appDbContext.Doctors.FirstOrDefaultAsync(d => d.RQE == rqe);
    }

}
