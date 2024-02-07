using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Selu383.SP24.Api.Data;
using Selu383.SP24.Api.Features.Hotels;
using Selu383.SP24.Api.Features.Roles;
using Selu383.SP24.Api.Features.UserRoles;
using Selu383.SP24.Api.Features.Users;

namespace Selu383.SP24.Api.Controllers
{
    [Route("api/Dummy")]
    [ApiController]
    public class DummyController : ControllerBase
    {
        private readonly DbSet<UserRole> userRoles;
        private readonly DbSet<Role> roles;
        private readonly DbSet<User> users;
        private readonly DbSet<Hotel> hotels;
        private readonly DataContext dataContext;

        public DummyController(DataContext dataContext)
        {
            this.dataContext = dataContext;
            hotels = dataContext.Set<Hotel>();
            users = dataContext.Set<User>();
            roles = dataContext.Set<Role>();
            userRoles = dataContext.Set<UserRole>();
        }







    }
}
