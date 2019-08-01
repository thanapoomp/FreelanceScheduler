using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using freelance.api.Data;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace freelance.api.Helper
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();
            var userId = int.Parse(resultContext.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var dbContext = resultContext.HttpContext.RequestServices.GetService<DataContext>();
            var user = await dbContext.Users.Where(x => x.Id == userId).FirstOrDefaultAsync();
            user.LastActive = DateTime.Now;
            await dbContext.SaveChangesAsync();
        }
    }
}