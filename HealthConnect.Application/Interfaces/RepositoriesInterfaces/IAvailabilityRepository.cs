using HealthConnect.Application.Dtos.Availability;
using HealthConnect.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Application.Interfaces.RepositoriesInterfaces;

public interface IAvailabilityRepository
{
    Task<bool> HasOverlappingAvailabilityAsync(Guid doctorId, DateTime newSlotStart, DateTime newSlotEnd);

    public Task<Availability> GetAvailabilityByIdAsync(Guid id);

    public Task<IEnumerable<TProjection>> GetAllAvailabilityPerDoctor<TProjection>(Guid doctorId);

    public Task CreateAvailabilityAsync(Availability availability);
}
