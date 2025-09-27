namespace HealthConnect.Infrastructure.Repositories;

using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Domain.Models;
using HealthConnect.Domain.Models.Specialities;
using HealthConnect.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// Repository for managing doctors and doctorsSpecialities assignments.
/// </summary>
public class DoctorRepository(
    AppDbContext appDbContext)
    : IDoctorRepository
{
    private readonly AppDbContext _appDbContext = appDbContext;

    /// <inheritdoc/>
    public async Task CreateDoctor(Doctor doctor)
    {
        await _appDbContext.Doctors.AddAsync(doctor);
    }

    /// <inheritdoc/>
    public IQueryable<Doctor> GetAllDoctors()
    {
        return _appDbContext.Doctors.AsNoTracking();
    }

    /// <inheritdoc/>
    public IQueryable<Doctor> GetAllDoctorsBySpecialityAsync(Guid specialityId)
    {
        return _appDbContext.Doctors.AsNoTracking();
    }

    /// <inheritdoc/>
    public async Task<Doctor?> GetDoctorById(Guid id)
    {
        return await _appDbContext.Doctors
            .Include(d => d.User)
                .ThenInclude(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(d => d.DoctorCRMs)
            .Include(d => d.DoctorSpecialities)
                .ThenInclude(ds => ds.Speciality)
            .FirstOrDefaultAsync(d => d.Id == id && d.User != null);
    }

    /// <inheritdoc/>
    public async Task<Doctor?> GetDoctorByUserId(Guid userId)
    {
        return await _appDbContext.Doctors
            .Include(d => d.User)
                .ThenInclude(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(d => d.DoctorCRMs)
            .Include(d => d.DoctorSpecialities)
                .ThenInclude(ds => ds.Speciality)
            .FirstOrDefaultAsync(d => d.UserId == userId);
    }

    /// <inheritdoc/>
    public async Task<Doctor?> GetDoctorByRQE(string rqe)
    {
        return await _appDbContext.Doctors
            .Include(d => d.User)
                .ThenInclude(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .Include(d => d.DoctorCRMs)
            .Include(d => d.DoctorSpecialities)
                .ThenInclude(ds => ds.Speciality)
            .FirstOrDefaultAsync(d => d.RQE == rqe);
    }

    /// <inheritdoc/>
    public async Task AddDoctorLinkToSpeciality(DoctorSpeciality doctorSpeciality)
    {
        await _appDbContext.DoctorSpecialities.AddAsync(doctorSpeciality);
    }

    /// <inheritdoc/>
    public void RemoveDoctorLinkToSpeciality(DoctorSpeciality doctorSpeciality)
    {
        _appDbContext.DoctorSpecialities.Remove(doctorSpeciality);
    }

    /// <inheritdoc/>
    public async Task<DoctorSpeciality?> GetDoctorSpecialityLink(Guid doctorId, Guid specialityId)
    {
        return await _appDbContext.DoctorSpecialities
            .FirstOrDefaultAsync(ds => ds.SpecialityId == specialityId && ds.DoctorId == doctorId);
    }
}
