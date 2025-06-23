namespace SharedKernel.Interfaces;

public interface IUnitOfWork : IDisposable
{
    Task CommitAsync(CancellationToken ct = default);
}
