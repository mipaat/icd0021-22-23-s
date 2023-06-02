using System.Security.Cryptography;
using App.BLL.Base;
using App.BLL.Contracts;
using App.BLL.Contracts.Services;
using App.DAL.DTO.Entities;
using App.DAL.DTO.Entities.Playlists;
using App.Common;
using Microsoft.Extensions.Logging;

namespace App.BLL.Services;

public class ImageService : BaseService<ImageService>, IImageService
{
    public ImageService(IServiceUow serviceUow, ILogger<ImageService> logger) : base(serviceUow, logger)
    {
    }

    public async Task UpdateProfileImages(Author author)
    {
        await UpdateImages(author.ProfileImages, AppPaths.GetProfileImagesDirectory(author.Platform),
            author.IdOnPlatform);
    }

    public async Task UpdateThumbnails(Video video)
    {
        await UpdateImages(video.Thumbnails, AppPaths.GetThumbnailsDirectory(video.Platform),
            video.IdOnPlatform);
    }

    public async Task UpdateThumbnails(Playlist playlist)
    {
        await UpdateImages(playlist.Thumbnails, AppPaths.GetThumbnailsDirectory(playlist.Platform),
            playlist.IdOnPlatform);
    }

    private async Task UpdateImages(ImageFileList? images, string downloadDirectory, string fileNamePrefix)
    {
        if (images == null || images.Count == 0) return;
        Utils.Utils.EnsureDirectoryExists(downloadDirectory);
        using var httpClient = new HttpClient();
        var imagesToRemove = new ImageFileList();
        foreach (var image in images)
        {
            var response = await httpClient.GetAsync(image.Url);
            if (!response.IsSuccessStatusCode)
            {
                imagesToRemove.Add(image);
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
            var filePath = Path.Combine(downloadDirectory,
                $"{fileNamePrefix}_{DateTime.UtcNow.Ticks}_{Guid.NewGuid().ToString().Replace("-", "")}.jpg");
            await using var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
            await fileStream.WriteAsync(imageBytes);
            image.LocalFilePath = filePath;
        }

        foreach (var image in imagesToRemove)
        {
            images.Remove(image);
        }
    }
}