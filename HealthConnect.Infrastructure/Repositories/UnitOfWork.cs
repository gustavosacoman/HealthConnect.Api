using HealthConnect.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthConnect.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _appDbContext;
    public UnitOfWork(AppDbContext context)
    {
        _appDbContext = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<int> SaveChangesAsync()
    {
        return await _appDbContext.SaveChangesAsync();
    }

    public void Dispose()
    {
        _appDbContext.Dispose();
        GC.SuppressFinalize(this);
    }

}
