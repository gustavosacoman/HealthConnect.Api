using HealthConnect.Domain.Models.Specialities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Application.Interfaces.RepositoriesInterfaces;

public interface ISpecialityRepository
{
    public Task<Speciality> GetSpecialityByIdAsync(Guid Id);

    public Task<IEnumerable<Speciality>> GetAllSpecialitiesAsync();

    public Task CreateSpecialityAsync(Speciality speciality);

    public Task<Speciality> GetSpecialityByNameAsync(string name);
}
