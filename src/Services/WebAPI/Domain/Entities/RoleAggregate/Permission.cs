// using Domain.Primitives;
//
// namespace Domain.Entities.RoleAggregate;
//
// public class Permission: BaseEntity
// {
//     public string Name { get; private set; }
//     public string Description { get; private set; }
//     
//     public ICollection<Role> Roles { get; private set; } = new List<Role>();
//
//
//     public Permission() // ORM
//     {
//     }
//
//     public Permission(Guid id, string name, string description) : base(id)
//     {
//         Name = name;
//         Description = description;
//     }
//
//     public static Permission Create(Guid id, string name, string description)
//     {
//         var permission = new Permission(id, name, description);
//         //raise domain event if needed
//
//         return permission;
//     }
//     
// }
