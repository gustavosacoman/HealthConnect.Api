using Microsoft.EntityFrameworkCore;

namespace HealthConnect.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }
}
