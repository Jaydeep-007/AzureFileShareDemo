namespace AzureFileShareDemo.Repositories
{
    public interface IFileShare
    {
        Task FileUploadAsync(FileDetails fileDetails);
        Task FileDownloadAsync(string fileShareName);
    }
}
