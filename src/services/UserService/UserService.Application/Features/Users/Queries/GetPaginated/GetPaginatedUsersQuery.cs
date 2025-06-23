using MediatR;
using SharedKernel.Queries;
using SharedKernel.Results;
using UserService.Application.Contracts.Models;

namespace UserService.Application.Features.Users.Queries.GetPaginated;

public sealed class GetPaginatedUsersQuery : PaginatedQuery<UserModel>
{
}
