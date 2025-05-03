﻿namespace firefly.Services.Interfaces
{
    public interface IStorageService
    {
        Task<string> UploadFromUrlAsync(string sourceUrl, string fileName, string contentType = "image/jpeg");
        string GetSasUrl(string blobPath, TimeSpan expiry);
    }
}
