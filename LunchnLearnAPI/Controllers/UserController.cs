using LunchnLearnAPI.Data;
using LunchnLearnAPI.Models.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace LunchnLearnAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly LunchandLearnDbContext _context;

        public UserController(LunchandLearnDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            return Ok(await _context.Users.ToListAsync());
        }

        [HttpGet]
        [Route("{AuthID}")]
        public async Task<IActionResult> GetUserByAuthID([Required, FromRoute] string authID)
        {
            var user = await _context.Users.FindAsync(authID);
            if (user != null)
            {
                return Ok(await _context.Users.Where(user => user.AuthID == authID).ToListAsync());
            }
            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> AddUser(string authID, string firstName, string lastName)
        {
            var userData = new Users()
            {
                AuthID = authID,
                FirstName = firstName,
                LastName = lastName,
            };
            await _context.Users.AddAsync(userData);
            await _context.SaveChangesAsync();

            return Ok(userData);
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateMeeting(
            [FromRoute] string id,
            string firstName,
            string lastName
        )
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.FirstName = firstName;
                user.LastName = lastName;

                await _context.SaveChangesAsync();

                return Ok(user);
            }
            return NotFound();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteMeetingByID([FromRoute] string id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Ok(user);
            }
            return NotFound();
        }
    }
}
