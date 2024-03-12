using onion_architecture.Api.Infrastructure.Middleware;

namespace onion_architecture.Api.Infrastructure.Extensions
{
    public static class ExceptionMiddlewareExtension
    {
        public static void UseCustomExceptionMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionMiddleware>();
        }
    }
}
