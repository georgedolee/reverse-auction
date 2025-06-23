namespace SharedKernel.Interfaces;

public interface IWritableRepository<T>
    where T : class
{
    Task AddAsync(T entity, CancellationToken ct = default);
}
