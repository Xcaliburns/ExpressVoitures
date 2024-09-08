using Microsoft.AspNetCore.Identity;

namespace DotnetProjet5.Areas.Identity.Models
{
        public class RoleEdit
    {
        public IdentityRole Role { get; set; }
        public IEnumerable<IdentityUser> Members { get; set; }
        public IEnumerable<IdentityUser> NonMembers { get; set; }
    }

}
