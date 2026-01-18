using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Azure.Storage.Sas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParcelService.Services.LandBank
{
    public class BlobStorageService
    {
        private readonly BlobContainerClient _container;

        public BlobStorageService(string connectionString, string containerName)
        {
            _container = new BlobContainerClient(connectionString, containerName);
            _container.CreateIfNotExists();
        }

        public async Task<string> UploadAsync(Stream fileStream,string fileName,string contentType,string parcelNumber)
        {
            var blobPath = $"{parcelNumber}/{Guid.NewGuid()}_{fileName}";
            var blobClient = _container.GetBlobClient(blobPath);

            using var memoryStream = new MemoryStream();
            await fileStream.CopyToAsync(memoryStream);
            memoryStream.Position = 0;

            await blobClient.UploadAsync(
                memoryStream,
                new BlobHttpHeaders { ContentType = contentType });

            var sasBuilder = new BlobSasBuilder
            {
                BlobContainerName = blobClient.BlobContainerName,
                BlobName = blobClient.Name,
                Resource = "b",
                ExpiresOn = DateTimeOffset.UtcNow.AddHours(24)
            };

            sasBuilder.SetPermissions(BlobSasPermissions.Read);

            return blobClient.GenerateSasUri(sasBuilder).ToString();
        }
    }

}
