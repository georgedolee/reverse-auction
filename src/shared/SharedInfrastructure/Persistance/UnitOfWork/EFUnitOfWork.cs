using Microsoft.EntityFrameworkCore;
using SharedKernel.Interfaces;

namespace SharedInfrastructure.Persistance.UnitOfWork;

public class EFUnitOfWork<TContext> : IUnitOfWork
    where TContext : DbContext
{
    private readonly TContext _context;

    public EFUnitOfWork(TContext context)
    {
        _context = context;
    }

    public async Task CommitAsync(CancellationToken ct = default)
    {
        await _context.SaveChangesAsync(ct);
    }

    public void Dispose()
    {
        _context.Dispose();
    }
}
