using CsvDataManager.Service;

namespace CsvDataManager.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<CsvDataSaveApiService>();

            services.AddScoped<FileProcessingService>();

            services.AddScoped<CsvDataRetrieveApiService>();

            return services;
        }
    }
}
