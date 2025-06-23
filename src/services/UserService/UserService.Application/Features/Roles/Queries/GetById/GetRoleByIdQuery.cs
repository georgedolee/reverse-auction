using MediatR;
using UserService.Application.Contracts.Models;

namespace UserService.Application.Features.Roles.Queries.GetById;

public sealed class GetRoleByIdQuery : IRequest<RoleModel>
{
    public GetRoleByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
