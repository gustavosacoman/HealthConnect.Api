namespace HealthConnect.Application.Interfaces;

/// <summary>
/// Represents a unit of work that coordinates the writing of changes and manages transactions.
/// </summary>
public interface IUnitOfWork : IAsyncDisposable
{
    /// <summary>
    /// Persists all changes made in the context to the underlying data store asynchronously.
    /// </summary>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation, with the number of state entries written to the data store.
    /// </returns>
    Task<int> SaveChangesAsync();
}
