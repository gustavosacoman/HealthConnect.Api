using HealthConnect.Domain.Models;

namespace HealthConnect.Application.Interfaces.RepositoriesInterfaces;

public interface IDoctorRepository
{
    public Task<IEnumerable<Doctor>> GetAllDoctors();

    public Task<Doctor?> GetDoctorById(Guid id);

    public Task<Doctor?> GetDoctorByRQE(string rqe);

    public Task CreateDoctor(Doctor doctor);

}
