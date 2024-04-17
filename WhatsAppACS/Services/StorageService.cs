using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using WhatsAppACS.Helpers;

namespace WhatsAppACS.Services
{
    public class StorageService : IStorageService
    {
        BlobContainerClient containerClient;

        public StorageService()
        {
            containerClient = new BlobContainerClient(
                Constants.AzureStorageConnectionString,
                Constants.AzureStorageDocsContainer);
        }

        public async Task<string> UploadBlob(FileResult file)
        {
            try
            {
                var fileExtension = Path.GetExtension(file.FileName);
                var contentType = file.ContentType;
                var blobHttpHeaders = new BlobHttpHeaders
                {
                    ContentType = file.ContentType
                };

                var newFileName = $"{Guid.NewGuid()}{fileExtension}";
                var blobClient = containerClient.GetBlobClient(newFileName);
                using var fileStream = await file.OpenReadAsync();
                await blobClient.UploadAsync(fileStream, blobHttpHeaders);

                return blobClient.Uri.AbsoluteUri;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}