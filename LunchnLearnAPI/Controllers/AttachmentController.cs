using LunchnLearnAPI.Data;
using LunchnLearnAPI.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;

namespace LunchnLearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttachmentController : ControllerBase
    {
        private readonly LunchandLearnDbContext _context;

        public AttachmentController(LunchandLearnDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("{meetingId:guid}")]
        public async Task<IActionResult> GetAttachments([FromRoute] Guid? meetingId)
        {
            if (_context.Attachments.Any(i => i.Meeting.MeetingID.Equals(meetingId)))
            {
                return Ok(
                    await _context.Attachments
                        .Include(i => i.Meeting)
                        .Where(attachment => attachment.Meeting.MeetingID.Equals(meetingId))
                        .ToListAsync()
                );
            }
            return NotFound();
        }

        [HttpGet]
        public async Task<IActionResult> GetAttachments()
        {
            return Ok(await _context.Attachments.Include(i => i.Meeting).ToListAsync());
        }

        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload(IFormFile file, Guid meetingId)
        {
            if (file == null || file.Length <= 0)
            {
                return BadRequest("Invalid file");
            }

            Meeting meeting = _context.Meetings.Find(meetingId);
            if (meeting == null)
            {
                return BadRequest("Invalid meeting!");
            }

            try
            {
                // save the file to storage and return a response

                string connectionString = Globals.BLOB_CONNECTION_STRING;
                // Parse the connection string and create a blob client
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Create a container to store the blobs
                string containerName = "attachments";
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);
                await container.CreateIfNotExistsAsync();

                // Create a block blob and upload the file data to it
                DateTime now = DateTime.Now;
                string blobName = now.ToString("u") + " " + file.FileName;
                string fileType = Path.GetExtension(file.FileName);
                Console.WriteLine(blobName);
                Console.WriteLine(fileType);
                CloudBlockBlob blockBlob = container.GetBlockBlobReference(blobName);
                using (var fileStream = file.OpenReadStream())
                {
                    await blockBlob.UploadFromStreamAsync(fileStream);
                }

                Console.WriteLine(blockBlob.Uri.ToString());
                Console.WriteLine(blockBlob.BlobType.ToString());

                var attachmentData = new Attachment()
                {
                    FileName = file.FileName,
                    BlobName = blobName,
                    FileType = fileType,
                    UploadDate = now,
                    PublicURI = blockBlob.Uri.ToString(),
                    Meeting = meeting,
                };

                await _context.Attachments.AddAsync(attachmentData);
                await _context.SaveChangesAsync();

                return Ok(attachmentData);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex);
            }
        }

        [HttpDelete]
        [Route("{attachmentId:guid}")]
        public async Task<IActionResult> DeleteAttachmentByID([FromRoute] Guid attachmentId)
        {
            if (_context.Attachments.Any(i => i.AttachmentId.Equals(attachmentId)))
            {
                Attachment attachment = _context.Attachments.Find(attachmentId);
                string connectionString = Globals.BLOB_CONNECTION_STRING;
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                string containerName = "attachments";
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);

                CloudBlockBlob blockBlob = container.GetBlockBlobReference(attachment.BlobName);
                if (await blockBlob.ExistsAsync())
                {
                    if (await blockBlob.DeleteIfExistsAsync())
                    {
                        _context.Attachments.Remove(attachment);
                        await _context.SaveChangesAsync();
                    }
                    else
                        return NotFound("Unable to delete file in blob storage");
                }
                else
                    return NotFound("Unable to locate file in blob storage");

                return Ok(attachment);
            }
            else
                return BadRequest("Invalid attachment");
        }
    }
}
