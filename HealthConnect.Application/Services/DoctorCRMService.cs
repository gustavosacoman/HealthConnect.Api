using AutoMapper;
using HealthConnect.Application.Dtos.DoctorCRM;
using HealthConnect.Application.Interfaces;
using HealthConnect.Application.Interfaces.RepositoriesInterfaces;
using HealthConnect.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Application.Services;

public class DoctorCRMService(
    IDoctorCRMRepository doctorCRMRepository,
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IDoctorRepository doctorRepository)
{
    private readonly IDoctorCRMRepository _doctorCRMRepository = doctorCRMRepository;
    private readonly IMapper _mapper = mapper;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IDoctorRepository _doctorRepository = doctorRepository;

    public async Task CreateCRMAsync(DoctorCRMRegistrationDto doctorCRMDto)
    {
        var exist = await _doctorCRMRepository.GetCRMByCodeAndState(doctorCRMDto.CRMNumber, doctorCRMDto.State);

        if (exist != null)
        {
            throw new ArgumentException($"The CRM {doctorCRMDto.CRMNumber}" +
                $" with a state {doctorCRMDto.State} already exist in database");
        }

        var doctor = await _doctorRepository.GetDoctorById(doctorCRMDto.DoctorId);

        var newDoctorCRM = new DoctorCRM
        {
            Id = Guid.NewGuid(),
            DoctorId = doctorCRMDto.DoctorId,
            CRMNumber = doctorCRMDto.CRMNumber,
            State = doctorCRMDto.State,
            Doctor = doctor,
        };

        await _doctorCRMRepository.CreateCRMAsync(newDoctorCRM);
        await _unitOfWork.SaveChangesAsync();
    }

    
}
