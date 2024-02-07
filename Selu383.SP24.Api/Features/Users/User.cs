using Microsoft.AspNetCore.Identity;
using Selu383.SP24.Api.Features.UserRoles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selu383.SP24.Api.Features;
using Selu383.SP24.Api.Features.Hotels;

namespace Selu383.SP24.Api.Features.Users
{
    public class User : IdentityUser <int>
    {
        public ICollection<UserRole> Roles { get; set; }

        public ICollection<Hotel> ManagerHotels { get; set; }


    }
}
