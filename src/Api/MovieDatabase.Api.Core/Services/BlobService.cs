using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

using MovieDatabase.Api.Core.Documents.Blobs;
using MovieDatabase.SharedKernel.Configurations;

namespace MovieDatabase.Api.Core.Services;

internal class BlobService(BlobServiceClient blobClient) : IBlobService
{
    public async Task<Blob> UploadBlob(string containerName, string fileExtension, Stream stream, CancellationToken cancellationToken = default)
    {
        var container = blobClient.GetBlobContainerClient(containerName);
        await container.CreateIfNotExistsAsync(PublicAccessType.Blob, cancellationToken: cancellationToken);

        var blob = new Blob();

        var fileName = $"{blob.Id}{fileExtension}";
        var blobInfo = await container.UploadBlobAsync(fileName, stream, cancellationToken);

        blob.Path = $"{container.Uri.AbsolutePath}/{fileName}";
        blob.Hash = Convert.ToBase64String(blobInfo.Value.ContentHash);
        blob.Name = fileName;
        blob.BaseBlobUri = GetBlobBaseUri();

        return blob;
    }

    public string GetBlobBaseUri()
    {
        return blobClient.Uri.GetLeftPart(UriPartial.Authority);
    }

    public string GetContentUrl(string blobId)
    {
        return $"{GetBlobBaseUri()}/{BlobStorageConfiguration.ContentContainerName}/{blobId}.jpg";
    }
}