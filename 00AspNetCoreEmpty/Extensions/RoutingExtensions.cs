using _00AspNetCoreEmpty.Middlewares;

namespace _00AspNetCoreEmpty.Extensions
{
    public static class RoutingExtensions
    {
        public static IApplicationBuilder UseMyRouting(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<RoutingMiddleware>();
        }
    }
}
