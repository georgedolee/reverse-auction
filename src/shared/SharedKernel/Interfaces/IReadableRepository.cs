using SharedKernel.Results;
using System.Linq.Expressions;

namespace SharedKernel.Interfaces;

public interface IReadableRepository<T>
    where T : class
{
    Task<T?> GetAsync(Guid id, CancellationToken ct = default);

    Task<IEnumerable<T>> GetPagedAsync(int pageNumber, int pageSize, CancellationToken ct = default);

    Task<IEnumerable<T>> GetPagedAsync(
        int pageNumber,
        int pageSize,
        IQueryable<T> query,
        CancellationToken ct = default);

    Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate, CancellationToken ct = default);

    Task<int> CountAsync(CancellationToken ct = default);
}