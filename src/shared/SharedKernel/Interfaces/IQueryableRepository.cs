using System.Linq.Expressions;

namespace SharedKernel.Interfaces;

public interface IQueryableRepository<T>
    where T : class
{
    IQueryable<T> Query(Expression<Func<T, bool>>? expression);
}