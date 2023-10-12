using DatingAppApi.Data;
using DatingAppApi.Interfaces;
using DatingAppApi.Services;
using Microsoft.EntityFrameworkCore;

namespace DatingAppApi.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.AddDbContext<DataContext>(item =>
            {
                item.UseSqlServer(config.GetConnectionString("defaultconnection"));
            });

            services.AddCors(p => p.AddPolicy("corspolicy", build =>
            {
                build.WithOrigins("http://localhost:4200", "https://localhost:4200").AllowAnyMethod().AllowAnyHeader();
            }));

            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            return services;
        }
    }
}
