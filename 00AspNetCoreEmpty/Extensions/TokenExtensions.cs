using _00AspNetCoreEmpty.Middlewares;

namespace _00AspNetCoreEmpty.Extensions
{
    public static class TokenExtensions
    {
        public static IApplicationBuilder UseToken(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<TokenMiddleware>();
        }
    }
}
