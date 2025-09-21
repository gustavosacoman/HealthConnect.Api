using HealthConnect.Domain.Models;

namespace HealthConnect.Application.Interfaces.RepositoriesInterfaces;

public interface IDoctorRepository
{
    public IQueryable<Doctor> GetAllDoctors();

    public Task<Doctor?> GetDoctorById(Guid id);

    public Task<Doctor?> GetDoctorByRQE(string rqe);

    public Task CreateDoctor(Doctor doctor);

    public IQueryable<Doctor> GetAllDoctorsBySpecialityAsync(Guid specialityId);

}
