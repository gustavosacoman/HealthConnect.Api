namespace HealthConnect.Infrastructure.Repositories;

using HealthConnect.Application.Interfaces;
using HealthConnect.Infrastructure.Data;
using System;
using System.Threading.Tasks;

/// <summary>
/// Provides a unit of work implementation for managing database transactions.
/// </summary>
public class UnitOfWork(AppDbContext context)
    : IUnitOfWork
{
    private readonly AppDbContext _appDbContext = context;

    /// <summary>
    /// Saves all changes made in this context to the database asynchronously.
    /// </summary>
    /// <returns>The number of state entries written to the database.</returns>
    public async Task<int> SaveChangesAsync()
    {
        return await _appDbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Disposes the database context asynchronously.
    /// </summary>
    /// <returns>A <see cref="ValueTask"/> that represents the asynchronous dispose operation.</returns>
    public async ValueTask DisposeAsync()
    {
        await _appDbContext.DisposeAsync();
    }
}
