using Microsoft.AspNetCore.Identity;
using Selu383.SP24.Api.Features.UserRoles;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selu383.SP24.Api.Features;

namespace Selu383.SP24.Api.Features.Users
{
    public class User : IdentityUser <int>
    {
        public ICollection<UserRole> Roles { get; set; }
    }
}
