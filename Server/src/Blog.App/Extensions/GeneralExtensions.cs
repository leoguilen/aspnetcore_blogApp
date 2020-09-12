using Microsoft.AspNetCore.Http;
using System;
using System.Linq;

namespace Medium.App.Extensions
{
    public static class GeneralExtensions
    {
        public static Guid GetUserId(this HttpContext httpContext)
        {
            if (httpContext?.User is null)
                return Guid.Empty;

            var userId = httpContext.User.Claims
                .Single(x => x.Type == "id").Value;

            return Guid.Parse(userId);
        }
    }
}
