using HealthConnect.Application.Dtos.Doctors;
using HealthConnect.Application.Interfaces.ServicesInterface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HealthConnect.Api.Controllers.v1;

[ApiController]
[Route("api/v1/[controller]")]
[Produces("application/json")]
[Authorize]
public class DoctorController(IDoctorService doctorService) : ControllerBase
{
    private readonly IDoctorService _doctorService = doctorService;

    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(DoctorSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDoctorById(Guid id)
    {
        var doctor = await _doctorService.GetDoctorByIdAsync(id);
        return Ok(doctor);
    }

    [HttpGet("detail/{id:guid}")]
    [ProducesResponseType(typeof(DoctorSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDoctorByIdDetail(Guid id)
    {
        var doctor = await _doctorService.GetDoctorByIdDetailAsync(id);
        return Ok(doctor);
    }

    [HttpGet("by-rqe/{rqe}")]
    [ProducesResponseType(typeof(DoctorSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetDoctorByRQE(string rqe)
    {
        var doctor = await _doctorService.GetDoctorByRQEAsync(rqe);
        return Ok(doctor);
    }

    [HttpGet("all")]
    [ProducesResponseType(typeof(IEnumerable<DoctorSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllDoctors()
    {
        var doctors = await _doctorService.GetAllDoctorsAsync();
        return Ok(doctors);
    }

    [HttpGet("by-Speciality/all/{specialityId:guid}")]
    [ProducesResponseType(typeof(IEnumerable<DoctorSummaryDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetAllDoctorsBySpeciality(Guid specialityId)
    {
        var doctors = await _doctorService.GetAllDoctorsBySpecialityAsync(specialityId);
        return Ok(doctors);
    }

    [HttpPatch]
    [ProducesResponseType(typeof(DoctorSummaryDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateDoctor(Guid id, DoctorUpdatingDto data)
    {
        var updatedDoctor = await _doctorService.UpdateDoctorAsync(id, data);
        return Ok(updatedDoctor);
    }
}
