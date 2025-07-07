using Microsoft.EntityFrameworkCore;
using SharedKernel.Interfaces;
using SharedKernel.Results;
using System.Linq.Expressions;

namespace SharedInfrastructure.Persistance.Repositories;

public class EFReadableRepository<T> : IReadableRepository<T>
    where T : class
{
    private readonly DbContext _context;

    public EFReadableRepository(DbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
    {
        return await _context.Set<T>()
                .Where(predicate)
                .ToListAsync(ct);
    }

    public async Task<T?> GetAsync(Guid id, CancellationToken ct = default)
    {
        return await _context.Set<T>()
                    .FindAsync(id, ct);
    }

    public async Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
    {
        var items = await _context.Set<T>()
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);

        return items;
    }

    public async Task<IEnumerable<T>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        IQueryable<T> query,
        CancellationToken ct = default)
    {
        return await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
    }

    public async Task<int> CountAsync(CancellationToken ct = default)
    {
        return await _context.Set<T>().CountAsync(ct);
    }
}
