using Domain.Entities.UserAggregate;

namespace Application.Abstractions;

public interface IJwtProvider
{
    string Generate(User user);
}