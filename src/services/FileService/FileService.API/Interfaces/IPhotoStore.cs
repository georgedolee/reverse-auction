namespace FileService.API.Interfaces;

public interface IPhotoStore
{
    Task<string> SaveAsync(Stream imageStream, string fileName, CancellationToken ct);

    bool DeleteAsync(string photoId, CancellationToken ct);
}
