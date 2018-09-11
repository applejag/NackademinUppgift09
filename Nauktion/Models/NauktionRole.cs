using Microsoft.AspNetCore.Identity;

namespace Nauktion.Models
{
    public class NauktionRole : IdentityRole
    {
        public NauktionRole()
        {
        }

        public NauktionRole(string roleName)
            : base(roleName)
        {
        }
    }
}