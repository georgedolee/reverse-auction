using MediatR;
using SharedKernel.Results;

namespace SharedKernel.Queries;

public class PaginatedQuery<T> : IRequest<PagedResult<T>>
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
