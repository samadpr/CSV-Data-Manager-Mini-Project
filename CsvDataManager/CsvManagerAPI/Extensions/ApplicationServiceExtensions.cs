using Domain.Models;
using Domain.Services.Login;
using Domain.Services.Login.Interface;
using Domain.Services.SignUp;
using Domain.Services.SignUp.Interface;
using Microsoft.EntityFrameworkCore;

namespace CsvManagerAPI.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<CsvDataManagerDbContext>(options => 
                options.UseSqlServer(config.GetConnectionString("DefaultConnection"))
            );
            
            services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);

            services.AddScoped<ISignUpRequestRepository, SignUpRequestRepository>();
            services.AddScoped<ISignUpRequestService, SignUpRequestService>();

            services.AddScoped<ILoginRequestRepository, LoginRequestRepository>();
            services.AddScoped<ILoginRequestService, LoginRequestService>();

            return services;
        }
    }
}
