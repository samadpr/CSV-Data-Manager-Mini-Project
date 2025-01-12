using CsvDataManager.Service;
using CsvDataManager.Service.Interface;

namespace CsvDataManager.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<CsvDataSaveApiService>();

            services.AddScoped<FileProcessingService>();

            services.AddScoped<ICsvDataRetrieveApiService, CsvDataRetrieveApiService>();

            return services;
        }
    }
}
