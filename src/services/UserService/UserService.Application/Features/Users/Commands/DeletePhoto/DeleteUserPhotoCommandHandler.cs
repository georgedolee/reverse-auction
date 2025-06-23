using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Photos;
using SharedKernel.Guards;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Users.Commands.DeletePhoto;

internal sealed class DeleteUserPhotoCommandHandler : IRequestHandler<DeleteUserPhotoCommand, bool>
{
    private readonly PhotoService.PhotoServiceClient _grpcClient;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<DeleteUserPhotoCommandHandler> _logger;

    public DeleteUserPhotoCommandHandler(
        PhotoService.PhotoServiceClient client,
        UserManager<User> userManager,
        ILogger<DeleteUserPhotoCommandHandler> logger)
    {
        _grpcClient = client;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteUserPhotoCommand command, CancellationToken ct)
    {
        _logger.LogInformation("Fetching user with id {UserId}.", command.Id);
        var user = await _userManager.FindByIdAsync(command.Id.ToString());

        Guard.EnsureFound(user, nameof(user), command.Id, _logger);

        if (user!.Photo == null)
        {
            _logger.LogInformation("User with id {UserId} doens't have a photo.", command.Id);
            return true;
        }

        _logger.LogInformation("Deleting photo of user with id {UserId}.", command.Id);
        var deleteReply = await _grpcClient.DeleteAsync(new DeleteRequest
        {
            Url = user.Photo
        }, cancellationToken: ct);

        if (!deleteReply.Success)
        {
            _logger.LogWarning("Photo of user with id {UserId} couldnot be deleted.", command.Id);
            throw new Exception("Photo could not be deleted.");
        }

        user.Photo = null;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            _logger.LogWarning("Photo of user with id {UserId} couldnot be deleted.", command.Id);
            throw new InvalidOperationException(result.Errors.First().Description);
        }

        _logger.LogInformation("Photo of user with id {UserId} deleted successfulyl.", command.Id);
        return true;
    }
}
