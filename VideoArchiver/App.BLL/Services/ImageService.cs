using System.Security.Cryptography;
using App.BLL.Base;
using App.Domain;
using App.Domain.NotMapped;
using Microsoft.Extensions.Logging;

namespace App.BLL.Services;

public class ImageService : BaseService<ImageService>
{
    public ImageService(ServiceUow serviceUow, ILogger<ImageService> logger) : base(serviceUow, logger)
    {
    }

    private const string DownloadsDirectory = "downloads";
    private static readonly string ThumbnailsDirectory = Path.Combine(DownloadsDirectory, "thumbnails");
    private static readonly string ProfileImagesDirectory = Path.Combine(DownloadsDirectory, "profile_images");

    private void EnsureDirectoryExists(string directory)
    {
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    public async Task UpdateProfileImages(Author author)
    {
        await UpdateImages(author.ProfileImages, ProfileImagesDirectory,
            $"{author.Platform}_{author.IdOnPlatform}_{DateTime.UtcNow.Ticks}");
    }

    public async Task UpdateThumbnails(Video video)
    {
        await UpdateImages(video.Thumbnails, ThumbnailsDirectory,
            $"{video.Platform}_{video.IdOnPlatform}_{DateTime.UtcNow.Ticks}");
    }

    public async Task UpdateThumbnails(Playlist playlist)
    {
        await UpdateImages(playlist.Thumbnails, ThumbnailsDirectory,
            $"{playlist.Platform}_{playlist.IdOnPlatform}_{DateTime.UtcNow.Ticks}");
    }

    private async Task UpdateImages(ImageFileList? images, string downloadDirectory, string fileName)
    {
        if (images == null || images.Count == 0) return;
        EnsureDirectoryExists(downloadDirectory);
        using var httpClient = new HttpClient();
        foreach (var image in images)
        {
            var response = await httpClient.GetAsync(image.Url);
            if (!response.IsSuccessStatusCode)
            {
                images.Remove(image);
                continue;
            }

            var isChanged = image.LocalFilePath == null;
            if (!isChanged && image.Etag != null && response.Headers.ETag != null)
            {
                if (image.Etag == response.Headers.ETag.Tag)
                {
                    continue;
                }

                isChanged = true;
            }

            var imageBytes = await response.Content.ReadAsByteArrayAsync();
            if (!isChanged && image.Hash != null)
            {
                var hash = MD5.HashData(imageBytes);
                var hashString = BitConverter.ToString(hash).Replace("-", "").ToLower();
                if (hashString == image.Hash)
                {
                    continue;
                }

                image.Hash = hashString;
                isChanged = true;
            }

            if (!isChanged) continue;
            var filePath = Path.Combine(downloadDirectory, fileName + ".jpg");
            await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            await fileStream.WriteAsync(imageBytes);
            image.LocalFilePath = filePath;
        }
    }
}