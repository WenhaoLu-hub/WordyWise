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
        return await _myContext.Set<User>().FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<User> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        
        throw new NotImplementedException();
    }

    public Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default)
    {
        // _myContext.Set<User>().FirstOrDefaultAsync(x=>x.Email=email, cancellationToken);
        throw new NotImplementedException();
    }

    public async Task<bool> IsPhoneNumberUniqueAsync(PhoneNumber phone, CancellationToken cancellationToken = default)
    {
        return !await _myContext.Set<User>().AnyAsync(x => x.PhoneNumber == phone, cancellationToken);
    }

    public void Add(User? user)
    {
        _myContext.Set<User>().Add(user);
    }

    public void Update(User user)
    {
        throw new NotImplementedException();
    }

    public void Delete(User user)
    {
        throw new NotImplementedException();
    }
}