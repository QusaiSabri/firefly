using Azure.Storage.Blobs;
using Azure.Storage.Sas;
using firefly.Services.Interfaces;


namespace firefly.Services.Storage
{
    public class AzureBlobStorageService : IStorageService
    {
        private readonly BlobServiceClient _blobServiceClient;
        private readonly string _containerName;

        public AzureBlobStorageService(IConfiguration config)
        {
            var connStr = config["AzureBlob:ConnectionString"];
            _blobServiceClient = new BlobServiceClient(connStr);
            _containerName = config["AzureBlob:ContainerName"];
        }

        public async Task<string> UploadFromUrlAsync(string sourceUrl, string fileName, string contentType = "image/jpeg")
        {
            var container = _blobServiceClient.GetBlobContainerClient(_containerName);
            await container.CreateIfNotExistsAsync();
            var blobClient = container.GetBlobClient(fileName);

            using var httpClient = new HttpClient();
            using var stream = await httpClient.GetStreamAsync(sourceUrl);

            var headers = new Azure.Storage.Blobs.Models.BlobHttpHeaders
            {
                ContentType = contentType
            };

            await blobClient.UploadAsync(stream, overwrite: true);

            return blobClient.Uri.ToString();
        }

          public string GetSasUrl(string blobPath, TimeSpan expiry)
    {
        var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);
        var blobClient = containerClient.GetBlobClient(blobPath);

        var sasUri = blobClient.GenerateSasUri(BlobSasPermissions.Read, DateTimeOffset.UtcNow.Add(expiry));
        return sasUri.ToString();
    }
    }
}
