using MediatR;
using UserService.Application.Contracts.Models;
using UserService.Application.Contracts.Requests;

namespace UserService.Application.Features.Users.Commands.Update;

public sealed class UpdateUserCommand 
    : UpdateUserRequest, IRequest<UserModel>
{
    public UpdateUserCommand(
        Guid id, 
        string userName, 
        string email)
    {
        Id = id;
        UserName = userName;
        Email = email;
    }

    public Guid Id { get; set; }
}