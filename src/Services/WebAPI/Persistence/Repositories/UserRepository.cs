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

    public Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return _myContext.Users.FirstOrDefaultAsync(x => x.Id == id);
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
        var user = await _myContext.Users.FirstOrDefaultAsync(x=>x.PhoneNumber == phone, cancellationToken);
        if (user != null)
        {
            return false;
        }
        return true;
    }

    public void Add(User user)
    {
        _myContext.Users.Add(user);
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