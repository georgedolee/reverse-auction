using FileService.API.Interfaces;
using System.Text.RegularExpressions;

namespace FileService.API.Services;

public class FileSystemPhotoStore : IPhotoStore
{
    private readonly string _uploadsRoot;

    public FileSystemPhotoStore(IWebHostEnvironment env)
    {
        _uploadsRoot = Path.Combine(env.ContentRootPath, "wwwroot", "uploads");

        if (!Directory.Exists(_uploadsRoot))
        {
            Directory.CreateDirectory(_uploadsRoot);
        }
    }

    public async Task<string> SaveAsync(Stream imageStream, string fileName, CancellationToken ct)
    {
        var safeFileName = Path.GetFileNameWithoutExtension(fileName);
        var extension = Path.GetExtension(fileName);
        safeFileName = Regex.Replace(safeFileName, @"[^a-zA-Z0-9_-]", "_");
        var uniqueName = $"{Guid.NewGuid()}_{safeFileName}{extension}";
        var fullPath = Path.Combine(_uploadsRoot, uniqueName);

        using var fs = new FileStream(fullPath, FileMode.CreateNew, FileAccess.Write);
        await imageStream.CopyToAsync(fs, ct);

        return $"/uploads/{uniqueName}";
    }

    public bool DeleteAsync(string photoId, CancellationToken ct)
    {
        var relativePath = photoId.TrimStart('/');
        var fullPath = Path.Combine(_uploadsRoot, Path.GetFileName(relativePath));

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }

        return true;
    }
}
