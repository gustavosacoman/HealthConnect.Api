namespace HealthConnect.Application.Interfaces.ServicesInterface;

using HealthConnect.Application.Dtos.Availability;

public interface IAvailabilityService
{
    public Task<IEnumerable<AvailabilitySummaryDto>> GetAllAvailabilitiesPerDoctorAsync(Guid doctorId);

    public Task<AvailabilitySummaryDto> CreateAvailabilityAsync(AvailabilityRegistrationDto availability);

    public Task<AvailabilitySummaryDto> GetAvailabilityByIdAsync(Guid availabilityId);

    public Task DeleteAvailabilityAsync(Guid availabilityId);

}
