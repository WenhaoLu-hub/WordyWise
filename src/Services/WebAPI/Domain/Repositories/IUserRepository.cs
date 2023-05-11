using Domain.Entities.UserAggregate;
using Domain.ValueObjects;

namespace Domain.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    
    Task<User> GetByEmailAsync(Email email, CancellationToken cancellationToken = default);

    Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default);

    Task<bool> IsPhoneNumberUniqueAsync(PhoneNumber phone, CancellationToken cancellationToken = default);

    void Add(User user);

    void Update(User user);

    void Delete(User user);

}