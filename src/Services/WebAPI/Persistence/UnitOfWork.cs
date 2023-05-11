using Domain.Repositories;

namespace Persistence;

public class UnitOfWork : IUnitOfWork
{
    private readonly MyContext _myContext;

    public UnitOfWork(MyContext myContext)
    {
        _myContext = myContext;
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _myContext.SaveChangesAsync(cancellationToken);
    }
}