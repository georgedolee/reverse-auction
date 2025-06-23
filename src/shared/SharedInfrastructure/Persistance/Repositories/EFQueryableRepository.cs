using Microsoft.EntityFrameworkCore;
using SharedKernel.Interfaces;
using System.Linq.Expressions;

namespace SharedInfrastructure.Persistance.Repositories;

public class EFQueryableRepository<T> : IQueryableRepository<T>
    where T : class
{
    private readonly DbContext _context;

    public EFQueryableRepository(DbContext context)
    {
        _context = context;
    }

    public IQueryable<T> Query(Expression<Func<T, bool>>? expression)
    {
        return expression == null ?
                _context.Set<T>().AsQueryable() :
                _context.Set<T>().Where(expression);
    }
}
