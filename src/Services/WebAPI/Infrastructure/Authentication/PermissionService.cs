using Domain.Entities.UserAggregate;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Infrastructure.Authentication;

public class PermissionService : IPermissionService
{
    private readonly MyContext _context;

    public PermissionService(MyContext context)
    {
        _context = context;
    }

    public async Task<HashSet<string>> GetPermissionsAsync(Guid userId)
    {
        var roles = await _context.Set<User>()
            .Include(x => x.Roles)
            .ThenInclude(x => x.Permissions)
            .Where(x=>x.Id == userId)
            .Select(x=>x.Roles)
            .ToArrayAsync();
        return roles
                .SelectMany(x => x)
                .SelectMany(x => x.Permissions)
                .Select(x=>x.Name)
                .ToHashSet();
    }
}