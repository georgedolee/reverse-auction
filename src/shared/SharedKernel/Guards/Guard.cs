using Microsoft.Extensions.Logging;
using SharedKernel.Exceptions;

namespace SharedKernel.Guards;

public static class Guard
{
    public static void EnsureUserOwnsResource<T>(T resourceOwnerId, T currentUserId, string resourceName, ILogger logger)
        where T : IEquatable<T>
    {
        if (!resourceOwnerId.Equals(currentUserId))
        {
            logger.LogWarning(
                "{ResourceName} owner Id: {OwnerId} did not match with incoming request user Id: {UserId}.",
                resourceName, resourceOwnerId, currentUserId);

            throw new ForbiddenException($"You are not authorized to modify this {resourceName.ToLower()}.");
        }
    }

    public static void EnsureFound<TResource, TId>(TResource? resource, string resourceName, TId resourceId, ILogger logger)
    {
        if (resource == null)
        {
            logger.LogWarning("{resource} with id: {resourceId} could not be found.", resourceName, resourceId);
            throw new NotFoundException($"{resourceName} with id: {resourceId} could not be found.");
        }
    }
}
