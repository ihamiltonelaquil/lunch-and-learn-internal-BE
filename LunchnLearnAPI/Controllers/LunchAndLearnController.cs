using LunchnLearnAPI.Data;
using LunchnLearnAPI.Models.Domain;
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
        [HttpPost]
        public async Task<IActionResult> AddMeeting(AddMeeting meeting)
        {
            var meetingData = new Meeting()
            {
                MeetingTime = DateTime.Now,
                CreatorName= meeting.CreatorName,
                Description= meeting.Description,
                Topic= meeting.Topic,
                LinkToSlides = meeting.LinkToSlides,
                TeamsLink = meeting.TeamsLink,
            };
            await _context.Meetings.AddAsync(meetingData);
            await _context.SaveChangesAsync();

            return Ok(meetingData);
        }
    }
}
