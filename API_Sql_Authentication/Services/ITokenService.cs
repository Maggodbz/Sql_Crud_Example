using API_Sql_Authentication.Models;

namespace API_Sql_Authentication.Services
{
    public interface ITokenService
    {
        string GenerateToken(TestUser validatedUser);
        Task<int> GetUserIdFromToken(HttpContext context);
    }
}