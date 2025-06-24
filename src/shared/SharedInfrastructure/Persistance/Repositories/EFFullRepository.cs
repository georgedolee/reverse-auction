using Microsoft.EntityFrameworkCore;
using SharedKernel.Interfaces;
using System.Linq.Expressions;

namespace SharedInfrastructure.Persistance.Repositories;

public class EFFullRepository<T> : IFullRepository<T>
    where T : class
{
    private readonly EFReadableRepository<T> _readable;
    private readonly EFWritableRepository<T> _writable;
    private readonly EFQueryableRepository<T> _queryable;
    private readonly EFRemovableRepository<T> _removable;

    public EFFullRepository(DbContext context)
    {
        _readable = new EFReadableRepository<T>(context);
        _writable = new EFWritableRepository<T>(context);
        _queryable = new EFQueryableRepository<T>(context);
        _removable = new EFRemovableRepository<T>(context);
    }

    public Task AddAsync(T entity, CancellationToken ct = default)
        => _writable.AddAsync(entity, ct);

    public Task<int> CountAsync(CancellationToken ct = default)
        => _readable.CountAsync(ct);
    public Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default)
        => _readable.FindAsync(predicate, ct);

    public Task<T?> GetAsync(Guid id, CancellationToken ct = default)
        => _readable.GetAsync(id, ct);

    public Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default)
        => _readable.GetPagedAsync(pageNumber, pageSize, ct);

    public Task<IEnumerable<T>> GetPagedAsync(
        int pageNumber, 
        int pageSize,
        IQueryable<T> query,
        CancellationToken ct = default)
        => _readable.GetPagedAsync(pageNumber, pageSize, query, ct);

    public IQueryable<T> Query(Expression<Func<T, bool>>? expression)
        => _queryable.Query(expression);

    public void Remove(T entity)
        => _removable.Remove(entity);
}
