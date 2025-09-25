using HealthConnect.Application.Dtos.DoctorCRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Application.Interfaces.ServicesInterface;

public interface IDoctorCRMService
{
    Task<DoctorCRMSummaryDto> GetCRMByIdAsync(Guid id);

    Task<DoctorCRMSummaryDto> GetCRMByCodeAndState(string crmNumber, string state);

    Task<IEnumerable<DoctorCRMSummaryDto>> GetAllCRMAsync();

    Task CreateCRMAsync(DoctorCRMRegistrationDto doctorCRMDto);
}
