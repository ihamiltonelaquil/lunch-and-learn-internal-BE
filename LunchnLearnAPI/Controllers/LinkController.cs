using LunchnLearnAPI.Data;
using LunchnLearnAPI.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LunchnLearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LinkController : ControllerBase
    {
        private readonly LunchandLearnDbContext _context;

        public LinkController(LunchandLearnDbContext context)
        {
            _context = context;
        }

        // GET: api/Link
        // Retrieves a list of all links, including the associated meeting
        [Route("/api/[controller]")]
        [HttpGet]
        public async Task<IActionResult> GetLinks()
        {
            return Ok(await _context.Links.Include(meeting => meeting.Meeting).ToListAsync());
        }

        // GET: api/Link/{meetingID:guid}
        // Retrieves a list of all links associated with a specific meeting by ID
        [Route("/api/[controller]/{meetingID:guid}")]
        [HttpGet]
        public async Task<IActionResult> GetLinkByID(Guid meetingID)
        {
            Meeting meeting = await _context.Meetings.FindAsync(meetingID);
            if (meeting == null)
            {
                return NotFound();
            }

            return Ok(
                await _context.Links
                    .Where(meeting => meeting.Meeting.MeetingID == meetingID)
                    .ToListAsync()
            );
        }

        // POST: api/Link/{meetingID:guid}
        // Adds a link to a specific meeting by ID
        [Route("/api/[controller]/{meetingID:guid}")]
        [HttpPost]
        public async Task<IActionResult> AddLink([FromRoute] Guid meetingID, string linkURL, string linkName)
        {
            Meeting meeting = await _context.Meetings.FindAsync(meetingID);
            if (meeting == null)
            {
                return BadRequest("Invalid meeting!");
            }
            var Link = new LinkContainer() { Link = linkURL, Meeting = meeting, Name = linkName };
            await _context.Links.AddAsync(Link);
            await _context.SaveChangesAsync();

            return Ok(Link);
        }

        // DELETE: api/Link/{linkID:guid}
        // Deletes a specific link by ID
        [HttpDelete]
        [Route("{linkID:guid}")]
        public async Task<IActionResult> DeleteMeetingByID([FromRoute] Guid linkID)
        {
            var link = await _context.Links.FindAsync(linkID);

            if (link != null)
            {
                _context.Links.Remove(link);
                await _context.SaveChangesAsync();

                return Ok(link);
            }
            return NotFound();
        }
    }
}
