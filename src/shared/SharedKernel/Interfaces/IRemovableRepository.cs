namespace SharedKernel.Interfaces;

public interface IRemovableRepository<T>
    where T : class
{
    void Remove(T entity);
}