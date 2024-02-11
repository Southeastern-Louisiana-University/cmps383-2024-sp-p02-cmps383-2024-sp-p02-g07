using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Selu383.SP24.Api.Data;
using Selu383.SP24.Api.Features.Users;
using Selu383.SP24.Api.Features.UserRoles;
using Selu383.SP24.Api.Features.Roles;
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
        public async Task<IActionResult> CreateUser(UserCreateDto userCreateDto) {

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


            var roles = await _roleManager.Roles.Select(x => x.Name).ToListAsync();
            foreach (var x in userCreateDto.Roles)
            {
                if (!roles.Contains(x))
                {
                    return BadRequest();
                }
            }

            var userToCreate = new User
            {
                UserName = userCreateDto.UserName
            };
            


            var result = await _userManager.CreateAsync(userToCreate, userCreateDto.Password);

            if (!result.Succeeded)
            {
                return BadRequest();
            }

            await _userManager.AddToRolesAsync(user, userCreateDto.Roles);

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
