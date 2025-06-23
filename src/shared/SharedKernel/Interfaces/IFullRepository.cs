namespace SharedKernel.Interfaces;

public interface IFullRepository<T> :
    IReadableRepository<T>,
    IWritableRepository<T>,
    IRemovableRepository<T>,
    IQueryableRepository<T>
    where T : class
{
}