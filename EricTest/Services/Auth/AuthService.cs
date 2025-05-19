using EricTest.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

namespace EricTest.Services
{
    public class AuthService : IAuthService
    {
        private static List<User> _users = new List<User>
        {
            new User
            {
                Id = new Guid("50753b3a-b861-4993-a9d3-ee508b2d11b5"),
                Email = "admin@admin.@com",
                Username = "admin",
                Password = "admin",
                Roles = new List<string> { "Admin", "User" }
            },
            new User
            {
                Id = new Guid("113a4a25-4e5c-42e4-94e0-9d9b759d228d"),
                Email = "eric@ave.com",
                Username = "user1",
                Password = "user1",
                Roles = new List<string> { "User" }
            }
        };

        public User Authenticate(string username, string password)
        {
            var user = _users.FirstOrDefault(u =>
                u.Username == username &&
                u.Password == password);

            return user;
        }

        public User GetUserById(Guid id)
        {
            var user = _users.FirstOrDefault(u => u.Id == id);
            return user;
        }

        public User GetUserByEmail(string email)
        {
            var user = _users.FirstOrDefault(u => u.Email.Equals(email, StringComparison.OrdinalIgnoreCase));
            return user;
        }

        public ClaimsPrincipal CreateClaimsPrincipal(User user)
        {
            if (user == null)
                return null;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username)
            };

            // Add all roles as claims
            foreach (var role in user.Roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            return new ClaimsPrincipal(identity);
        }
    }
}
