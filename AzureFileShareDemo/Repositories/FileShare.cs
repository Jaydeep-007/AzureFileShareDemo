using Azure;
using Azure.Storage.Files.Shares;
using Azure.Storage.Files.Shares.Models;

namespace AzureFileShareDemo.Repositories
{
    public class FileShare : IFileShare
    {
        private readonly IConfiguration _config;

        public FileShare(IConfiguration config)
        {
            _config = config;
        }

        public async Task FileUploadAsync(FileDetails fileDetails)
        {
            // Get the configurations and create share object
            ShareClient share = new ShareClient(_config.GetConnectionString("default"), _config.GetValue<string>("FileShareDetails:FileShareName"));

            // Create the share if it doesn't already exist
            await share.CreateIfNotExistsAsync();

            // Check the file share is present or not
            if (await share.ExistsAsync())
            {
                // Get a reference to the sample directory
                ShareDirectoryClient directory = share.GetDirectoryClient("FileShareDemoFiles");

                // Create the directory if it doesn't already exist
                await directory.CreateIfNotExistsAsync();

                // Ensure that the directory exists
                if (await directory.ExistsAsync())
                {
                    // Get a reference to a file and upload it
                    ShareFileClient file = directory.GetFileClient(fileDetails.FileDetail.FileName);

                    // Check path
                    var filesPath = Directory.GetCurrentDirectory() + "/files";
                    var fileName = Path.GetFileName(fileDetails.FileDetail.FileName);
                    var filePath = Path.Combine(filesPath, fileName);

                    using (FileStream stream = File.OpenRead(filePath))
                    {
                        file.Create(stream.Length);
                        file.UploadRange(
                            new HttpRange(0, stream.Length),
                            stream);
                    }
                }
            }
            else
            {
                Console.WriteLine($"File is not upload successfully");
            }
        }

        public async Task FileDownloadAsync(string fileShareName)
        {
            ShareClient share = new ShareClient(_config.GetConnectionString("default"), _config.GetValue<string>("FileShareDetails:FileShareName"));
            ShareDirectoryClient directory = share.GetDirectoryClient("FileShareDemoFiles");
            ShareFileClient file = directory.GetFileClient(fileShareName);

            // Check path
            var filesPath = Directory.GetCurrentDirectory() + "/files";
            if (!System.IO.Directory.Exists(filesPath))
            {
                Directory.CreateDirectory(filesPath);
            }

            var fileName = Path.GetFileName(fileShareName);
            var filePath = Path.Combine(filesPath, fileName);

            // Download the file
            ShareFileDownloadInfo download = file.Download();
            using (FileStream stream = File.OpenWrite(filePath))
            {
                await download.Content.CopyToAsync(stream);
            }
        }
    }
}
