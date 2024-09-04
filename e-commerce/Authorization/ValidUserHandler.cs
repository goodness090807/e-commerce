using e_commerce.Data;
using e_commerce.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.Authorization
{
    /// <summary>
    /// 驗證使用者是否為有效使用者
    /// </summary>
    public class ValidUserHandler : AuthorizationHandler<IAuthorizationRequirement>
    {
        private readonly ApplicationDbContext _context;

        public ValidUserHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, IAuthorizationRequirement requirement)
        {
            var userId = context.User.GetUserId();
            if (await _context.Users.AnyAsync(x => x.Id == userId && x.Valid))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
        }
    }
}
