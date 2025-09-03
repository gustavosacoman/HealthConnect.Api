namespace HealthConnect.Application.Interfaces.ServicesInterface;

using HealthConnect.Application.Dtos.Availability;

public interface IAvailabilityService
{
    public Task<AvailabilitySummaryDto> GetAllAvailabilitiesPerDoctor(Guid doctorId);

    public Task<AvailabilitySummaryDto> CreateAvailability(AvailabilityRegistrationDto availability);

    public Task<AvailabilitySummaryDto> GetAvailabilityById(Guid availabilityId);

    public Task DeleteAvailability(Guid availabilityId);

}
