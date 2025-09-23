using HealthConnect.Domain.Models;

namespace HealthConnect.Application.Interfaces.RepositoriesInterfaces;

public interface IDoctorCRMRepository
{
    Task<DoctorCRM?> GetCRMByCodeAndState(string code, string state);

    IQueryable<DoctorCRM> GetAllAsync();

    Task<DoctorCRM?> GetByIdAsync(Guid id);

    Task CreateCRMAsync(DoctorCRM doctorCRM);
}
