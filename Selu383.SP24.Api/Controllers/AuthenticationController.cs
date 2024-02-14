using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Selu383.SP24.Api.Data;
using Selu383.SP24.Api.Features.Dtos;
using Selu383.SP24.Api.Features.Hotels;
using Selu383.SP24.Api.Features.Users;
using System.Security.Claims;

namespace Selu383.SP24.Api.Controllers
{
    [Route("/api/authentication")]
    public class AuthenticationController : Controller
    {
        private readonly DataContext dataContext;
        private readonly SignInManager<User> signInManager;
        private readonly UserManager<User> userManager;
        private readonly DbSet<User> users;



        public AuthenticationController(DataContext dataContext)
        {
            this.dataContext = dataContext;

        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            if (!IsValidCredentials(loginDto.UserName, loginDto.Password))
            {
                return BadRequest("Invalid username or password.");
            }

            string token = GenerateJwtToken(loginDto.UserName);

            UserDto userDto = await GetUserInfo(loginDto.UserName);

            return Ok(new { Token = token, User = userDto });
        }

        private Task<UserDto> GetUserInfo(string userName)
        {
            throw new NotImplementedException();
        }

        private string GenerateJwtToken(string userName)
        {
            throw new NotImplementedException();
        }

        private bool IsValidCredentials(string userName, string password)
        {
            throw new NotImplementedException();
        }
 
     


        
    }
}
