using SharedKernel.Validators;
using UserService.Application.Contracts.Models;

namespace UserService.Application.Features.Users.Queries.GetPaginated;

public sealed class GetPaginatedUsersQueryValidator 
    : PaginatedQueryValidator<GetPaginatedUsersQuery, UserModel>
{
}
