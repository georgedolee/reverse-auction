using Google.Protobuf;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Photos;
using SharedKernel.Guards;
using UserService.Domain.Entities;

namespace UserService.Application.Features.Users.Commands.UploadPhoto;

internal sealed class UploadUserPhotoCommandHandler : IRequestHandler<UploadUserPhotoCommand, bool>
{
    private readonly PhotoService.PhotoServiceClient _grpcClient;
    private readonly UserManager<User> _userManager;
    private readonly ILogger<UploadUserPhotoCommandHandler> _logger;

    public UploadUserPhotoCommandHandler(
        PhotoService.PhotoServiceClient client,
        UserManager<User> userManager,
        ILogger<UploadUserPhotoCommandHandler> logger)
    {
        _grpcClient = client;
        _userManager = userManager;
        _logger = logger;
    }

    public async Task<bool> Handle(UploadUserPhotoCommand command, CancellationToken ct)
    {
        _logger.LogInformation("Fetching user with id {UserId}.", command.Id);
        var user = await _userManager.FindByIdAsync(command.Id.ToString());

        Guard.EnsureFound(user, nameof(user), command.Id, _logger);

        if (user!.Photo != null)
        {
            _logger.LogInformation("Deleting photo of user with id {UserId}.", command.Id);
            var deleteReply = await _grpcClient.DeleteAsync(new DeleteRequest
            {
                Url = user.Photo
            }, cancellationToken: ct);

            if (!deleteReply.Success)
            {
                _logger.LogInformation("Photo of user with id {UserId} couldnot be deleted.", command.Id);
                throw new Exception("Photo could not be deleted.");
            }
        }

        _logger.LogInformation(
            "Uploading photo with filename {FileName} for user with id {UserId}", 
            command.File.FileName, command.Id);

        await using var ms = new MemoryStream();
        await command.File.CopyToAsync(ms, ct);

        var uploadReply = await _grpcClient.SaveAsync(new UploadRequest
        {
            Data = ByteString.CopyFrom(ms.ToArray()),
            Filename = command.File.FileName
        }, cancellationToken: ct);

        user.Photo = uploadReply.Url;
        var result = await _userManager.UpdateAsync(user);

        if (!result.Succeeded)
        {
            _logger.LogWarning(
                "Photo with filename {FileName} couldnot be uploaded for user with id {UserId}", 
                command.File.FileName, command.Id);
            throw new InvalidOperationException(result.Errors.First().Description);
        }

        _logger.LogInformation(
            "Photo with filename {FileName} uploaded successfully for user with id {UserId}", 
            command.File.FileName, command.Id);
        return result.Succeeded;
    }
}