using Domain.Entities.UserAggregate;
using Domain.Repositories;
using Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly MyContext _myContext;

    public UserRepository(MyContext myContext)
    {
        _myContext = myContext;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _myContext.Set<User>().Include(u => u.Roles).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {

        return await _myContext.Set<User>().FirstOrDefaultAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await _myContext.Set<User>().AnyAsync(x => x.Email == email, cancellationToken);
    }

    public async Task<bool> IsPhoneNumberUniqueAsync(PhoneNumber phone, CancellationToken cancellationToken = default)
    {
        return await _myContext.Set<User>().AnyAsync(x => x.PhoneNumber == phone, cancellationToken);
    }

    public void AddLoginHistory(UserLoginHistory userLoginHistory)
    {
        _myContext.Set<UserLoginHistory>().Add(userLoginHistory);
    }
    
    public void RemoveRole(User user, Role role)
    {
        throw new NotImplementedException();
    }

    public void Add(User user)
    {
        _myContext.Set<User>().Add(user);
    }

    public void Update(User user)
    {
        _myContext.Set<User>().Update(user);
    }

    public void Delete(User user)
    {
        _myContext.Set<User>().Remove(user);
    }

    public async Task<Role?> FindRoleById(int roleId, CancellationToken cancellationToken = default)
    {
        return await _myContext.Set<Role>().FirstOrDefaultAsync(x=> x.Id == roleId, cancellationToken);
    }
}