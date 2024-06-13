using Microsoft.AspNetCore.Authorization;

namespace web_table.Web.Services
{
    public class RoleRequirement : IAuthorizationRequirement
    {
        public string Role { get; private set; }

        public RoleRequirement(string role)
        {
            Role = role;
        }
    }
}
