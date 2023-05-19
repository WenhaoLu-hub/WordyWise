using System.Reflection;
using Domain.Entities.UserAggregate;
using Microsoft.EntityFrameworkCore;

namespace Persistence;

public class MyContext : DbContext
{
    public DbSet<User> Users { get; set; } 
    public MyContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly());
    }
}