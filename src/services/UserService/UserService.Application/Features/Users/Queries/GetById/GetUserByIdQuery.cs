using MediatR;
using UserService.Application.Contracts.Models;

namespace UserService.Application.Features.Users.Queries.GetById;

public sealed class GetUserByIdQuery : IRequest<UserModel>
{
    public GetUserByIdQuery(Guid id)
    {
        Id = id;
    }

    public Guid Id { get; set; }
}
