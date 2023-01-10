using LunchnLearnAPI.Data;
using LunchnLearnAPI.Models.Domain;
using LunchnLearnAPI.Models.Domain.Meetings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.IO;

namespace LunchnLearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LunchAndLearnController : ControllerBase
    {
        private readonly LunchandLearnDbContext _context;

        public LunchAndLearnController(LunchandLearnDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMeetings()
        {
            return Ok(await _context.Meetings.ToListAsync());
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetMeetingByID([FromRoute] Guid id)
        {
            var meeting = await _context.Meetings.FindAsync(id);

            if (meeting != null)
            {
                return Ok(await _context.Meetings.Where(meeting => meeting.MeetingID == id).ToListAsync());
            }
            return NotFound();
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> GetMeetingByName([FromRoute] string name)
        {
            if (_context.Meetings.Count(i => i.CreatorName.Contains(name)) > 0)
            {
                return Ok(await _context.Meetings.Where(meeting => meeting.CreatorName.Contains(name)).ToListAsync());
            }
            return NotFound();

        }

        [HttpGet]
        [Route("attachments")]
        public async Task<IActionResult> GetAttachments([FromRoute] Guid? meetingId)
        {
            if(meetingId == null)
            {
                return Ok(await _context.Attachments.ToListAsync());
            }
            else if (_context.Attachments.Any(i => i.Meeting.Equals(meetingId)))
            {
                return Ok(await _context.Attachments.Where(attachment => attachment.Meeting.Equals(meetingId)).ToListAsync());
            }
            return NotFound();
        } 


        [HttpPost]
        [Route("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length <= 0)
            {
                return BadRequest("Invalid file");
            }

            try
            {
                // save the file to storage and return a response
                
                string connectionString = "DefaultEndpointsProtocol=https;AccountName=lunchandlearnblob;AccountKey=7OPCs2Yxtfz3BzJCnS3tfpNIR2+vJf7Kzx2Km30dFL8kq46zOWF/ZY3PbQ5gOuv/Ib+3IfDS91wg+AStjRYA2Q==;EndpointSuffix=core.windows.net";
                // Parse the connection string and create a blob client
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connectionString);
                CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

                // Create a container to store the blobs
                string containerName = "attachments";
                CloudBlobContainer container = blobClient.GetContainerReference(containerName);
                await container.CreateIfNotExistsAsync();

                // Create a block blob and upload the file data to it
                DateTime now = DateTime.Now;
                string blobName = now.ToString("O") + " " + file.FileName;
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
                    Meeting = null,
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

        [HttpPost]
        public async Task<IActionResult> AddMeeting(AddMeeting meeting)
        {
            var meetingData = new Meeting()
            {
                MeetingStart = DateTime.Now,
                MeetingEnd = DateTime.Now,
                CreatorName = meeting.CreatorName,
                Description = meeting.Description,
                Topic = meeting.Topic,
                LinkToSlides = meeting.LinkToSlides,
                TeamsLink = meeting.TeamsLink,
            };
            await _context.Meetings.AddAsync(meetingData);
            await _context.SaveChangesAsync();

            return Ok(meetingData);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateMeeting([FromRoute] Guid id, UpdateMeeting updateMeeting)
        {
            var meeting = await _context.Meetings.FindAsync(id);

            if (meeting != null)
            {
                meeting.MeetingStart = updateMeeting.MeetingStart;
                meeting.MeetingEnd = updateMeeting.MeetingEnd;
                meeting.CreatorName = updateMeeting.CreatorName;
                meeting.Description = updateMeeting.Description;
                meeting.Topic = updateMeeting.Topic;
                meeting.TeamsLink = updateMeeting.TeamsLink;
                meeting.LinkToSlides = updateMeeting.LinkToSlides;

                await _context.SaveChangesAsync();

                return Ok(meeting);
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteMeetingByID([FromRoute] Guid id)
        {
            var meeting = await _context.Meetings.FindAsync(id);

            if (meeting != null)
            {
                _context.Meetings.Remove(meeting);
                await _context.SaveChangesAsync();

                return Ok(meeting);
            }
            return NotFound();
        }
    }
}
