using Domain.Entities.UserAggregate;
using Domain.Primitives;

namespace Domain.Entities.RoleAggregate;

public class Role : AggregateRoot
{
    public string Name { get; private set; }

    public ICollection<UserRole> UserRoles { get; private set; } = new List<UserRole>();

    public ICollection<Permission> Permissions { get; private set; } = new List<Permission>();


    private Role() //ORM
    {

    }

    public Role(Guid id, string name) : base(id)
    {
        Name = name;
    }

    public static Role Create(Guid id, string name)
    {
        var role = new Role(id, name);
        //raise domain event if needed
        return role;
    }

}
    