using Microsoft.AspNetCore.Identity;
using Selu383.SP24.Api.Features.Users;
using Selu383.SP24.Api.Features.Roles;

namespace Selu383.SP24.Api.Features.UserRoles
{
    public class UserRole : IdentityUserRole<int>
    {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }

    }
}
