using _00AspNetCoreEmpty.Middlewares;

namespace _00AspNetCoreEmpty.Extensions
{
    public static class AuthenticationExtensions
    {
        public static IApplicationBuilder UseTokenAuthentication(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
