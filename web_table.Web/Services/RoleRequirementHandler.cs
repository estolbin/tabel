using Microsoft.AspNetCore.Authorization;

namespace web_table.Web.Services
{
    public class RoleRequirementHandler : AuthorizationHandler<RoleRequirement>
    {
        private readonly CurrentUserProvider _userProvider;

        public RoleRequirementHandler(CurrentUserProvider userProvider)
        {
            _userProvider = userProvider;
        }

        protected override async Task<Task> HandleRequirementAsync(AuthorizationHandlerContext context, RoleRequirement requirement)
        {
            if(await _userProvider.UserInRole(requirement.Role))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
