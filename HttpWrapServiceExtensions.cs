using Microsoft.Extensions.DependencyInjection;

namespace Thunder.HttpClientWrap
{
    public static class HttpWrapServiceExtensions
    {
        public static IServiceCollection AddNoty(this IServiceCollection services) => services.AddScoped<INoty, Logger>();
        public static IServiceCollection AddHttpWrap(this IServiceCollection services) => services.AddScoped<HttpWrap>();
        public static IServiceCollection AddAuthentication(this IServiceCollection services) => services.AddScoped<AuthenticationService>();
    }
}
