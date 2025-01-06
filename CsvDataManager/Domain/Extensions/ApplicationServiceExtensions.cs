using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<CsvDataManagerDbContext>(options =>
                options.UseSqlServer(config.GetConnectionString("Data Source=ABDUL-SAMAD;Initial Catalog=CsvDataManagerDb;Integrated Security=True;Trust Server Certificate=True"))
            );

            return services;
        }
    }
}
