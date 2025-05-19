using EricTest.Models;
using System.Security.Claims;

namespace EricTest.Services
{
    public interface IAuthService
    {
        User Authenticate(string username, string password);
        User GetUserById(Guid id);
        User GetUserByEmail(string email);
        ClaimsPrincipal CreateClaimsPrincipal(User user);
    }
}
