using Domain.Primitives;

namespace Domain.Entities.UserAggregate;

public class Role : Enumeration<Role>
{
    public static readonly Role Registered = new Role(1, "Registered");
    public Role(int id, string name) : base(id, name)
    {
    }

    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();

    public ICollection<User> Users { get; set; } = new List<User>();

}