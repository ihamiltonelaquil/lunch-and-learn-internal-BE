using LunchnLearnAPI.Data;
using LunchnLearnAPI.Models.Domain;
using LunchnLearnAPI.Models.Domain.Meetings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;

namespace LunchnLearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MeetingController : ControllerBase
    {
        private readonly LunchandLearnDbContext _context;

        // Injecting LunchandLearnDbContext
        public MeetingController(LunchandLearnDbContext context)
        {
            _context = context;
        }

        // GET: api/Meeting
        // This endpoint returns all Meetings
        [HttpGet]
        public async Task<IActionResult> GetMeetings()
        {
            return Ok(await _context.Meetings.ToListAsync());
        }

        // GET: api/Meeting/{name}
        // This endpoint returns Meetings with CreatorName containing the passed name
        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> GetMeetingByName([FromRoute] string name)
        {
            return Ok(
                await _context.Meetings
                    .Where(
                        meeting =>
                            _context
                                .FuzzySearch(meeting.CreatorName)
                                .Contains(_context.FuzzySearch(name))
                    )
                    .ToListAsync()
            );
        }

        // GET: api/Meeting/{id:guid}
        // This endpoint returns Meeting with the passed id
        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> GetMeetingByID([FromRoute] Guid id)
        {
            var meeting = await _context.Meetings.FindAsync(id);
            if (meeting != null)
            {
                return Ok(
                    await _context.Meetings
                        .Where(meeting => meeting.MeetingID == id)
                        .OrderBy(name => name.CreatorName)
                        .ToListAsync()
                );
            }
            return NotFound();
        }

        // POST: api/Meeting
        // This endpoint adds a new Meeting
        [HttpPost]
        public async Task<IActionResult> AddMeeting(AddMeeting meeting)
        {
            var meetingData = new Meeting()
            {
                AuthID = meeting.AuthID,
                MeetingStart = DateTime.Now,
                MeetingEnd = DateTime.Now,
                CreatorName = meeting.CreatorName,
                Description = meeting.Description,
                Topic = meeting.Topic,
            };
            await _context.Meetings.AddAsync(meetingData);
            await _context.SaveChangesAsync();

            return Ok(meetingData);
        }

        // PUT: api/Meeting/{id:guid}
        // This endpoint updates a Meeting with the passed id
        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateMeeting(
            [FromRoute] Guid id,
            UpdateMeeting updateMeeting
        )
        {
            var meeting = await _context.Meetings.FindAsync(id);
            if (meeting != null)
            {
                meeting.MeetingStart = (updateMeeting.MeetingStart != null ? updateMeeting.MeetingStart : meeting.MeetingStart);
                meeting.MeetingEnd = (updateMeeting.MeetingEnd != null ? updateMeeting.MeetingEnd : meeting.MeetingEnd);
                meeting.CreatorName = (updateMeeting.CreatorName != null ? updateMeeting.CreatorName : meeting.CreatorName);
                meeting.Description = (updateMeeting.Description != null ? updateMeeting.Description : meeting.Description);
                meeting.Topic = (updateMeeting.Topic != null ? updateMeeting.Topic : meeting.Topic);

                await _context.SaveChangesAsync();

                return Ok(meeting);
            }
            return NotFound();
        }

        // DELETE: api/Meeting/{id:guid}
        // This endpoint deletes a Meeting with the passed id
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteMeetingByID([FromRoute] Guid id)
        {
            var meeting = await _context.Meetings.FindAsync(id);

            if (meeting != null)
            {
                var attachments = await _context.Attachments.Where(a => a.Meeting.MeetingID == id).ToListAsync();
                if (attachments.Any())
                {
                    foreach (var attachment in attachments)
                    {
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
                                continue;
                            }
                            else
                                continue;
                        }
                        else
                            continue;

                    }
                }

                var links = await _context.Links.Where(a => a.Meeting.MeetingID == id).ToListAsync();
                if (links.Any())
                {
                    foreach(var link in links)
                    {
                        _context.Links.Remove(link);
                    }
                }

                _context.Meetings.Remove(meeting);
                await _context.SaveChangesAsync();

                return Ok(meeting);
            }
            return NotFound();
        }
    }
}
