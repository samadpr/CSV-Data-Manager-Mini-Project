using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using QueueWorkerConsole.CsvDataReceiver;
using QueueWorkerConsole.Service;
using RabbitMQ.Client;
using System;

namespace MyApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var csvReceiver = host.Services.GetRequiredService<QueueCsvDataReceiver>();

            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            csvReceiver.Receiver(channel);
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((_, services) =>
                {
                    services.AddHttpClient();
                    services.AddTransient<FileDataSaveApiService>();
                    services.AddTransient<QueueCsvDataReceiver>();
                });
    }
}