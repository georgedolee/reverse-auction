using Microsoft.EntityFrameworkCore;
using SharedKernel.Interfaces;

namespace SharedInfrastructure.Persistance.Repositories;

public class EFWritableRepository<T> : IWritableRepository<T>
    where T : class
{
    private readonly DbContext _context;

    public EFWritableRepository(DbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(T entity, CancellationToken ct = default)
    {
        await _context.Set<T>()
            .AddAsync(entity, ct);
    }
}
