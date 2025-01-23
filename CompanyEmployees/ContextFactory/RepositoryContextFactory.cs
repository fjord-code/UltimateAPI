using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Repository;

namespace CompanyEmployees.ContextFactory;

public class RepositoryContextFactory : IDesignTimeDbContextFactory<RepositoryContext>
{
    public RepositoryContext CreateDbContext(string[]? args = null)
    {
        var builder = new DbContextOptionsBuilder<RepositoryContext>()
            .UseSqlite(Environment.GetEnvironmentVariable("SqliteConnectionString"),
                options => options.MigrationsAssembly("CompanyEmployees"));

        return new RepositoryContext(builder.Options);
    }
}
