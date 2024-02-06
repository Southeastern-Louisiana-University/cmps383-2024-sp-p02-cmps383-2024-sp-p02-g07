using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Selu383.SP24.Api.Features;

namespace Selu383.SP24.Api.Features.Roles
{
    public class Role : IdentityRole<int>
    {
        public ICollection<UserRole> Users { get; set; }
    }
}
