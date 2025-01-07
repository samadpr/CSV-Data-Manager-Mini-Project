using QueueWorkerConsole.CsvDataReceiver;
using RabbitMQ.Client;
using System;

namespace MyApp
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var factory = new ConnectionFactory
            {
                HostName = "localhost",
                UserName = "guest",
                Password = "guest"
            };

            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            QueueCsvDataReceiver.Receiver(channel);
        }
    }
}