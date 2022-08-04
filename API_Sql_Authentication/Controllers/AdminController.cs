using API_Sql_Authentication.Models;
using API_Sql_Authentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API_Sql_Authentication.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly DbLoginSampleContext _context;
        private readonly ITokenService _tokenService;
        public AdminController(DbLoginSampleContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult<TestUser>> GetUserAsync()
        {
            int id = await _tokenService.GetUserIdFromToken(HttpContext);
            return await _context.TestUser.FirstOrDefaultAsync(x => x.Id == id);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> UpdateUserAsync(TestUser user)
        {
            int id = await _tokenService.GetUserIdFromToken(HttpContext);
            if (id != user.Id)
            {
                return BadRequest();
            }
            _context.Entry(user).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        [HttpDelete("[action]/{id}")]
        public async Task<ActionResult<TestUser>> DeleteUserAsync()
        {
            int id = await _tokenService.GetUserIdFromToken(HttpContext);
            var user = await _context.TestUser.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            _context.TestUser.Remove(user);
            await _context.SaveChangesAsync();
            return user;
        }

        private bool UserExists(int id)
        {
            return _context.TestUser.Any(e => e.Id == id);
        }

    }
}