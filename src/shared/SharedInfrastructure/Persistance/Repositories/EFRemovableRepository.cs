using Microsoft.EntityFrameworkCore;
using SharedKernel.Interfaces;

namespace SharedInfrastructure.Persistance.Repositories;

public class EFRemovableRepository<T> : IRemovableRepository<T>
    where T : class
{
    private readonly DbContext _context;

    public EFRemovableRepository(DbContext context)
    {
        _context = context;
    }

    public void Remove(T entity)
    {
        _context.Set<T>()
            .Remove(entity);
    }
}
