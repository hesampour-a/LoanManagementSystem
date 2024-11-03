using Microsoft.EntityFrameworkCore;

namespace LoanManagementSystem.Persistence.Ef;

public class EfDataContext(DbContextOptions<EfDataContext> options)
    : DbContext(options)
{
    public EfDataContext(
        string connectionString)
        : this(
            new DbContextOptionsBuilder<EfDataContext>()
                .UseSqlServer(connectionString).Options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(EfDataContext)
            .Assembly);
    }
}