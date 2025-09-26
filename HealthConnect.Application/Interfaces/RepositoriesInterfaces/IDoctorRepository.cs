using HealthConnect.Domain.Models;
using HealthConnect.Domain.Models.Specialities;

namespace HealthConnect.Application.Interfaces.RepositoriesInterfaces;

public interface IDoctorRepository
{
    public IQueryable<Doctor> GetAllDoctors();

    public Task<Doctor?> GetDoctorById(Guid id);

    public Task<Doctor?> GetDoctorByRQE(string rqe);

    public Task CreateDoctor(Doctor doctor);

    public IQueryable<Doctor> GetAllDoctorsBySpecialityAsync(Guid specialityId);

    public Task AddDoctorLinkToSpeciality(DoctorSpeciality doctorSpeciality);

    public Task RemoveDoctorLinkToSpeciality(DoctorSpeciality doctorSpeciality);

    public Task<DoctorSpeciality> GetDoctorSpecialityLink(Guid doctorId, Guid specialityId);

}
