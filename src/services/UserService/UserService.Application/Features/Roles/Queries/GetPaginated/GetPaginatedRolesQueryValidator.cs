using SharedKernel.Validators;
using UserService.Application.Contracts.Models;

namespace UserService.Application.Features.Roles.Queries.GetPaginated;

public sealed class GetPaginatedRolesQueryValidator 
    : PaginatedQueryValidator<GetPaginatedRolesQuery, RoleModel>
{
}
