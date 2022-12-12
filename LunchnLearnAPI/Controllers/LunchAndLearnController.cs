using LunchnLearnAPI.Data;
using LunchnLearnAPI.Models.Domain;
using LunchnLearnAPI.Models.Domain.Meetings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost]
        public async Task<IActionResult> AddMeeting(AddMeeting meeting)
        {
            var meetingData = new Meeting()
            {
                MeetingTime = DateTime.Now,
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
