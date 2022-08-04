using Microsoft.AspNetCore.Mvc;
using API_Sql_Authentication.Models;
using Microsoft.EntityFrameworkCore;
using API_Sql_Authentication.Services;

namespace API_Sql_Authentication.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly DbLoginSampleContext _context;
        private readonly ITokenService _tokenService;
        public AuthenticationController(DbLoginSampleContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        [HttpGet]
        [Route("[action]")]
        public async Task<ActionResult<AccessTokenModel>> LoginAsync(string username, string password)
        {
            TestUser user = await _context.TestUser.FirstOrDefaultAsync(x => x.Username == username && x.Password == password);
            if (user == null)
            {
                return null;
            }
            else
            {
                string token = _tokenService.GenerateToken(user);
                return new AccessTokenModel(token);

            }
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<ActionResult<TestUser>> RegisterAsync(TestUser user)
        {
            _context.TestUser.Add(user);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetUserAsync", new { id = user.Id }, user);
        }
    }
}