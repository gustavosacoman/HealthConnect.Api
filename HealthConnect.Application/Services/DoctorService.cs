using AutoMapper;
using HealthConnect.Application.Dtos.Doctors;
using HealthConnect.Application.Dtos.Users;
using HealthConnect.Application.Interfaces;
using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Application.Interfaces.ServicesInterface;
using HealthConnect.Domain.Models;

namespace HealthConnect.Application.Services;

public class DoctorService(
    IDoctorRepository doctorRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork) : IDoctorService
{
    private readonly IDoctorRepository _doctorRepository = doctorRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<IEnumerable<DoctorSummaryDto>> GetAllDoctorsAsync()
    {
        var doctors = await _doctorRepository.GetAllDoctors();
        return _mapper.Map<IEnumerable<DoctorSummaryDto>>(doctors);
    }

    public async Task<DoctorSummaryDto> GetDoctorByRQEAsync(string rqe)
    {
        if (string.IsNullOrWhiteSpace(rqe))
        {
            throw new NullReferenceException("RQE cannot be null or empty.");
        }

        var doctor = await _doctorRepository.GetDoctorByRQE(rqe)
            ?? throw new KeyNotFoundException($"Doctor with RQE {rqe} not found.");

        return _mapper.Map<DoctorSummaryDto>(doctor);
    }

    public async Task<DoctorDetailDto> GetDoctorByIdDetailAsync(Guid id)
    {
       if (id == Guid.Empty)
        {
            throw new NullReferenceException("ID cannot be empty.");
        }

       var doctor = await  _doctorRepository.GetDoctorById(id)
            ?? throw new KeyNotFoundException($"Doctor with ID {id} not found.");

       if (doctor.UserId == null)
        {
            throw new InvalidOperationException($"Doctor with ID {id} does not have an associated user.");
        }

       return _mapper.Map<DoctorDetailDto>(doctor);
    }

    public async Task<DoctorSummaryDto> GetDoctorByIdAsync(Guid id)
    {
        if (id == Guid.Empty)
        {
            throw new NullReferenceException("ID cannot be empty.");
        }

        var doctor = await _doctorRepository.GetDoctorById(id)
             ?? throw new KeyNotFoundException($"Doctor with ID {id} not found.");

        if (doctor.UserId == null)
        {
            throw new InvalidOperationException($"Doctor with ID {id} does not have an associated user.");
        }

        return _mapper.Map<DoctorSummaryDto>(doctor);
    }

    public async Task<DoctorSummaryDto> UpdateDoctorAsync(Guid id, DoctorUpdatingDto doctorUpdatingDto)
    {
        if (id == Guid.Empty)
        {
            throw new NullReferenceException("ID cannot be empty.");
        }

        var doctor = await _doctorRepository.GetDoctorById(id)
            ?? throw new KeyNotFoundException($"Doctor with ID {id} not found.");

        doctor.RQE = doctorUpdatingDto.RQE ?? doctor.RQE;
        doctor.CRM = doctorUpdatingDto.CRM ?? doctor.CRM;
        doctor.Biography = doctorUpdatingDto.Biography ?? doctor.Biography;

        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<DoctorSummaryDto>(doctor);
    }
}
