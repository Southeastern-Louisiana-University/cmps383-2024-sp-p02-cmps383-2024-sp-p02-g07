/*using Microsoft.AspNetCore.Mvc;

namespace Selu383.SP24.Api.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<ActionResult<UserDto>> CreateUser(CreateUserDto createUserDto)
        {
            // Check if roles provided are valid roles in your system
            if (!AreRolesValid(createUserDto.Roles))
            {
                return BadRequest("Invalid role(s) provided.");
            }

            // Check if at least one role is provided
            if (createUserDto.Roles == null || createUserDto.Roles.Count == 0)
            {
                return BadRequest("At least one role must be provided.");
            }

            // Check if the username is unique
            if (await _userService.IsUsernameTakenAsync(createUserDto.Username))
            {
                return BadRequest("Username is already taken.");
            }

            // Create the user
            var user = new User
            {
                Username = createUserDto.Username,
                Roles = createUserDto.Roles,
                // Include other properties from CreateUserDto
            };

            // Save the user
            _userService.AddUser(user);
            _userService.SaveChanges();

            // Return the created user
            var userDto = new UserDto
            {
                Id = user.Id,
                Username = user.Username,
                Roles = user.Roles,
                // Include other relevant user details
            };

            return Ok(userDto);
        }

        // Helper method to check if roles provided are valid roles in your system
        private bool AreRolesValid(List<string> roles)
        {
            // Implement your logic to check if each role in the list is a valid role
            // Return true if all roles are valid, otherwise return false
        }
    }
}
*/