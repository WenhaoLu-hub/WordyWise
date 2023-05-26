using Domain.Primitives;

namespace Domain.Entities.UserAggregate;

public class UserLoginHistory : BaseEntity
{
    public Guid UserId { get; private set; }
    public DateTime LoginTime { get; private set; }

    private UserLoginHistory() //ORM
    {
    }

    public UserLoginHistory(Guid id, Guid userId) : base(id)
    {
        UserId = userId;
        LoginTime = DateTime.UtcNow;
    }

    public static UserLoginHistory Create(Guid id, Guid userId)
    {
        var userLoginHistory = new UserLoginHistory(id, userId);
        //raise domain event if needed
        return userLoginHistory;
    }
}