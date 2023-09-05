using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage;
using EllipticCurve.Utils;

namespace EPIDIWeb.Common
{
    public class FileUploadHelpers
    {
        private IConfiguration Configuration;

        public FileUploadHelpers(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }
        public async Task<string> UploadFiles(IFormFile files)
        {
            var accountName = this.Configuration.GetSection("DocumentUploadCred")["BlobAccountName"];
            var accountKey = this.Configuration.GetSection("DocumentUploadCred")["BlobAccountKey"];
            var containerName = this.Configuration.GetSection("DocumentUploadCred")["Blobcontainername"];
            var fileUrl = this.Configuration.GetSection("DocumentUploadCred")["FileUrl"];
            var cloudStorageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
            var cloudBlobClient = cloudStorageAccount.CreateCloudBlobClient();
            var cloudBlobContainer = cloudBlobClient.GetContainerReference(containerName);
            var cloudBlockBlob = cloudBlobContainer.GetBlockBlobReference(Convert.ToString(files.FileName));

            await using var memoryStream = new MemoryStream();
            await files.CopyToAsync(memoryStream);
            var streamBytes = memoryStream.ToArray();

            using (Stream stream = new MemoryStream(streamBytes))
            {
                await cloudBlockBlob.UploadFromStreamAsync(stream);
            }
            string filePath_front = fileUrl + Convert.ToString(files.FileName);
            return filePath_front;
        }
    }
}
