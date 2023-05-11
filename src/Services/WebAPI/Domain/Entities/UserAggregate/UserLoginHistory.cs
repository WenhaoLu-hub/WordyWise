using Domain.Primitives;

namespace Domain.Entities.UserAggregate;

public class UserLoginHistory : BaseEntity
{
    public Guid UserId { get; private set; }
    public DateTime LoginTime { get; private set; }

    private UserLoginHistory() //ORM
    {
    }

    public UserLoginHistory(Guid id, Guid userId, DateTime loginTime) : base(id)
    {
        UserId = userId;
        LoginTime = loginTime;
    }

    public static UserLoginHistory Create(Guid id, Guid userId, DateTime loginTime)
    {
        var userLoginHistory = new UserLoginHistory(id, userId, loginTime);
        //raise domain event if needed
        return userLoginHistory;
    }
}