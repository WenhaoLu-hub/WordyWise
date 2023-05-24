using System.Reflection;
using Domain.Entities.UserAggregate;
using Domain.Repositories;
using Domain.ValueObjects;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Persistence.Infrastructure;

namespace Persistence.Repositories;

public class CachedUserRepository : IUserRepository
{
    private readonly UserRepository _userRepository;
    private readonly IDistributedCache _distributedCache;
    private readonly MyContext _context;

    public CachedUserRepository(UserRepository userRepository, IDistributedCache distributedCache, MyContext context)
    {
        _userRepository = userRepository;
        _distributedCache = distributedCache;
        _context = context;
    }

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var key = $"user-{id.ToString()}";
        var cachedUser = await _distributedCache.GetStringAsync(key,cancellationToken);
        User? user;
        if (string.IsNullOrEmpty(cachedUser))
        {
            user = await _userRepository.GetByIdAsync(id, cancellationToken);
            if (user is null)
            {
                return user;
            }

            await _distributedCache.SetStringAsync(key, 
                JsonConvert.SerializeObject(user), 
                cancellationToken);
            return user;
        }
        user = JsonConvert.DeserializeObject<User>(cachedUser,
            new JsonSerializerSettings
            {
                ContractResolver = new PrivateResolver(),
                ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor
            });
        if (user is not null)
        {
            _context.Set<User>().Attach(user);
        }
        return user;
    }

    public async Task<User?> GetByEmailAsync(Email email, CancellationToken cancellationToken = default)
    {
        //To do, implement redis cache
        return await _userRepository.GetByEmailAsync(email, cancellationToken);
    }

    public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default)
    {
        return await _userRepository.IsEmailUniqueAsync(email, cancellationToken);
    }

    public async Task<bool> IsPhoneNumberUniqueAsync(PhoneNumber phone, CancellationToken cancellationToken = default)
    {
        return await _userRepository.IsPhoneNumberUniqueAsync(phone, cancellationToken);
    }

    public void Add(User? user)
    {
        _userRepository.Add(user);
    }

    public void Update(User user)
    {
        _userRepository.Update(user);
    }

    public void Delete(User user)
    {
        _userRepository.Delete(user);
    }
}