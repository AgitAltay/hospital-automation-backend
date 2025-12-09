using Hospital.Application.Services;
using Hospital.Application.Services.Implementations;
using Microsoft.Extensions.DependencyInjection;

namespace Hospital.Application.Extensions
{
    public static class ApplicationServiceRegistration
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthAppService, AuthAppService>();
            
        }
    }
}