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
    public Task<Availability> GetAvailabilityByIdAsync(Guid id);

    public IQueryable<Availability> GetAllAvailabilityPerDoctor(Guid doctorId);

    public Task CreateAvailabilityAsync(Availability availability);
}
