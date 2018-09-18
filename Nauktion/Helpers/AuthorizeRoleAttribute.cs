using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Nauktion.Models;

namespace Nauktion.Helpers
{
    public class AuthorizeRoleAttribute : AuthorizeAttribute
    {
        public AuthorizeRoleAttribute(params NauktionRoles[] roles)
        {
            Roles = string.Join(',', roles.Select(role => role.ToString()));
        }
    }
}