using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Persistence;

public class DesignTimeDbFactory : IDesignTimeDbContextFactory<MyContext>
{
    public MyContext CreateDbContext(string[] args)
    {
        var dbConnectionString = Environment.GetEnvironmentVariable("DBConnectionStringDev");
        var dbContextOptionsBuilder = new DbContextOptionsBuilder<MyContext>();
        dbContextOptionsBuilder.UseSqlServer(dbConnectionString);
        return new MyContext(dbContextOptionsBuilder.Options);
    }
}