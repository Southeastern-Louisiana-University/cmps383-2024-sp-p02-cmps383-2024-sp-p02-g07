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

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Selu383.SP24.Api.Data;
using Selu383.SP24.Api.Features.Users;
using Selu383.SP24.Api.Features;
using Microsoft.EntityFrameworkCore;

namespace Selu383.SP24.Api.Controllers
{
    [Route("/api/users")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DataContext dataContext;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;

        public UsersController(UserManager<User> userManager, RoleManager<Role> roleManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser(UserCreateDto userCreateDto)
        {

            if (userCreateDto.UserName == null || userCreateDto.Password == null)
            {
                return BadRequest();
            }


            var user = await _userManager.FindByNameAsync(userCreateDto.UserName);
            if (user != null)
            {
                return BadRequest();
            }


            if (!userCreateDto.Roles.Any())
            {
                return BadRequest();
            }


            //var roles = await _roleManager.Roles.Select(x => x.Name).ToListAsync();
            //foreach (var x in userCreateDto.Roles)
            //{
            //    if (!roles.Contains(x))
            //    {
            //        return BadRequest();
            //    }
            //}

            var userToCreate = new User
            {
                UserName = userCreateDto.UserName
            };



            var result = await _userManager.CreateAsync(userToCreate, userCreateDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            //await _userManager.AddToRolesAsync(user, userCreateDto.Roles);

            var userToReturn = new UserDto
            {
                Id = user.Id,
                UserName = userToCreate.UserName,
                Roles = userCreateDto.Roles,

            };

            return Ok(userToReturn);
        }
    }


}