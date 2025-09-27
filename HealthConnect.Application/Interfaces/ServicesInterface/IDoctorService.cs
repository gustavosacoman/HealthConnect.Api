using HealthConnect.Application.Dtos.Doctors;

namespace HealthConnect.Application.Interfaces.ServicesInterface;

public interface IDoctorService
{
    public Task<IEnumerable<DoctorSummaryDto>> GetAllDoctorsAsync();

    public Task<DoctorDetailDto> GetDoctorByIdDetailAsync(Guid id);

    public Task<DoctorSummaryDto> GetDoctorByRQEAsync(string rqe);

    public Task<DoctorSummaryDto> GetDoctorByIdAsync(Guid id);

    public Task<DoctorSummaryDto> UpdateDoctorAsync(Guid id, DoctorUpdatingDto doctorUpdatingDto);

    public Task<IEnumerable<DoctorDetailDto>> GetAllDoctorsBySpecialityAsync(Guid specialityId);

    public Task<DoctorDetailDto> GetDoctoDetailByUserIdAsync(Guid userId);
}
