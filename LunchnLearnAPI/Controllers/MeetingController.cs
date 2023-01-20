using LunchnLearnAPI.Data;
using LunchnLearnAPI.Models.Domain;
using LunchnLearnAPI.Models.Domain.Meetings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
                _context.Meetings.Remove(meeting);
                await _context.SaveChangesAsync();

                return Ok(meeting);
            }
            return NotFound();
        }
    }
}
