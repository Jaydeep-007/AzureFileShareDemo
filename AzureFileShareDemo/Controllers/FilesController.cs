using AzureFileShareDemo.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AzureFileShareDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        private readonly IFileShare fileShare;

        public FilesController(IFileShare fileShare)
        {
            this.fileShare = fileShare;
        }

        /// <summary>
        /// upload file
        /// </summary>
        /// <param name="fileDetail"></param>
        /// <returns></returns>
        [HttpPost("Upload")]
        public async Task<IActionResult> UploadFile([FromForm] FileDetails fileDetail)
        {
            if (fileDetail.FileDetail != null)
            {
                await fileShare.FileUploadAsync(fileDetail);
            }
            return Ok();
        }

        /// <summary>
        /// download file
        /// </summary>
        /// <param name="fileDetail"></param>
        /// <returns></returns>
        [HttpPost("Download")]
        public async Task<IActionResult> DownloadFile(string fileName)
        {
            if (fileName != null)
            {
                await fileShare.FileDownloadAsync(fileName);
            }
            return Ok();
        }
    }
}
