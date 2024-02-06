using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Selu383.SP24.Api.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user = await _userService.GetUserByUsernameAsync(loginDto.Username);

            if (user == null || !VerifyPassword(user, loginDto.Password))
            {
                return BadRequest("Invalid username or password.");
            }

            // Create and return a UserDto (customize based on your UserDto structure)
            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                // Include other relevant user details
            };

            // Assuming you have a method to generate a JWT token
            var token = GenerateJwtToken(user);

            return Ok(new { User = userDto, Token = token });
        }

        [HttpGet("me")]
        [Authorize] // Requires authentication
        public ActionResult<UserDto> GetCurrentUser()
        {
            // Get the current user from the claims
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = _userService.GetUserById(userId);

            if (user == null)
            {
                return Unauthorized();
            }

            // Create and return a UserDto (customize based on your UserDto structure)
            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                // Include other relevant user details
            };

            return Ok(userDto);
        }

        [HttpPost("logout")]
        [Authorize] // Requires authentication
        public IActionResult Logout()
        {
            // Log out the user (you may clear any authentication cookies or tokens)

            // Return success
            return Ok();
        }

        // Helper method to verify password
        private bool VerifyPassword(User user, string password)
        {
            // Implement your password verification logic here
            // You might use a secure password hashing library like BCrypt
            return user.PasswordHash == HashPassword(password, user.Salt);
        }

        // Helper method to generate JWT token
        private string GenerateJwtToken(User user)
        {
            // Implement your JWT token generation logic here
            // Use a library like System.IdentityModel.Tokens.Jwt
            // Include necessary user claims (e.g., user id, username)
            // Set the expiration, issuer, etc.
            // Return the generated token
        }

        // Example: HashPassword using BCrypt
        private string HashPassword(string password, string salt)
        {
            return BCrypt.Net.BCrypt.HashPassword(password, salt);
        }
    }
}
