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

    public IQueryable<Doctor> GetAllDoctors()
    {
        return _appDbContext.Doctors.AsNoTracking();
    }

    public IQueryable<Doctor> GetAllDoctorsBySpecialityAsync(Guid specialityId)
    {
        return _appDbContext.Doctors.AsNoTracking();
    }

    public async Task<Doctor?> GetDoctorById(Guid id)
    {
        return await _appDbContext.Doctors
            .Include(d => d.User)
            .ThenInclude(U => U.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(d => d.DoctorCRMs)
            .Include(d => d.Speciality)
            .FirstOrDefaultAsync(d => d.Id == id && d.User != null);
    }

    public async Task<Doctor?> GetDoctorByRQE(string rqe)
    {
        return await _appDbContext.Doctors
            .Include(d => d.Speciality)
            .Include(d => d.User)
            .ThenInclude(u => u.UserRoles)
            .ThenInclude(ur => ur.Role)
            .Include(d => d.DoctorCRMs)
            .FirstOrDefaultAsync(d => d.RQE == rqe);
    }

}
