namespace HealthConnect.Application.Interfaces.RepositoriesInterfaces;

using HealthConnect.Application.Dtos.Availability;
using HealthConnect.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Repository interface for managing doctor availability slots.
/// </summary>
public interface IAvailabilityRepository
{
    /// <summary>
    /// Checks if there is any overlapping availability for a doctor within the specified time range.
    /// </summary>
    /// <param name="doctorId">The unique identifier of the doctor.</param>
    /// <param name="newSlotStart">The start time of the new slot.</param>
    /// <param name="newSlotEnd">The end time of the new slot.</param>
    /// <returns>True if there is an overlap; otherwise, false.</returns>
    Task<bool> HasOverlappingAvailabilityAsync(Guid doctorId, DateTime newSlotStart, DateTime newSlotEnd);

    /// <summary>
    /// Gets the availability for a doctor at a specific date and time.
    /// </summary>
    /// <param name="doctorId">The unique identifier of the doctor.</param>
    /// <param name="slotDateTime">The date and time of the slot.</param>
    /// <returns>The availability if found; otherwise, null.</returns>
    Task<Availability?> GetAvailabilityByDoctorIdDateAscyn(Guid doctorId, DateTime slotDateTime);

    /// <summary>
    /// Gets the availability by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the availability.</param>
    /// <returns>The availability.</returns>
    Task<Availability> GetAvailabilityByIdAsync(Guid id);

    /// <summary>
    /// Gets all availabilities for a doctor, projected to the specified type.
    /// </summary>
    /// <typeparam name="TProjection">The type to project the availabilities to.</typeparam>
    /// <param name="doctorId">The unique identifier of the doctor.</param>
    /// <returns>A collection of projected availabilities.</returns>
    Task<IEnumerable<TProjection>> GetAllAvailabilityPerDoctor<TProjection>(Guid doctorId);

    /// <summary>
    /// Creates a new availability slot.
    /// </summary>
    /// <param name="availability">The availability to create.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task CreateAvailabilityAsync(Availability availability);

    /// <summary>
    /// Deletes an existing availability slot.
    /// </summary>
    /// <param name="availability">The availability to delete.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    Task DeleteAvailability(Availability availability);
}
