using RabbitMQ.Client.Events;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Newtonsoft.Json;
using QueueWorkerConsole.Dto;
using System.Net.Http.Json;
using QueueWorkerConsole.Service;

namespace QueueWorkerConsole.CsvDataReceiver
{
    public class QueueCsvDataReceiver
    {
        private readonly FileDataSaveApiService _fileDataSaveApiService;

        public QueueCsvDataReceiver(FileDataSaveApiService fileDataSaveApiService)
        {
            _fileDataSaveApiService = fileDataSaveApiService;
        }

        public void Receiver(IModel channel)
        {
            channel.QueueDeclare(
                queue: "csv-data-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            Console.WriteLine("Waiting for messages from 'csv-data-queue'...");

            var receiver = new EventingBasicConsumer(channel);
            receiver.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);

                Console.WriteLine($"Received message: {message}");

                try
                {
                    var csvData = JsonConvert.DeserializeObject<CsvFileModelDto>(message)
                                  ?? throw new Exception("Invalid message format");


                    Console.WriteLine("Processing CSV rows...");
                    var rows = csvData.Data.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                    foreach (var row in rows)
                    {
                        var fileData = new FileDataDto
                        {
                            FileId = csvData.FileId,
                            Data = row
                        };

                        PrintFileData(fileData);

                        if (!await _fileDataSaveApiService.SaveCsvDataAsync(fileData))
                        {
                            Console.WriteLine("Failed to save file data. Rejecting message.");
                            channel.BasicReject(ea.DeliveryTag, false);
                            return;
                        }
                    }

                    channel.BasicAck(ea.DeliveryTag, false);
                    Console.WriteLine("Message processed successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                    channel.BasicReject(ea.DeliveryTag, false);
                }
            };

            channel.BasicConsume(
                queue: "csv-data-queue",
                autoAck: false, 
                consumer: receiver
            );

            Console.WriteLine("Press [Enter] to exit.");
            Console.ReadLine();
        }

        private static void PrintFileData(FileDataDto fileData)
        {
            Console.WriteLine("Processing FileDataDto...");
            Console.WriteLine($"FileId: {fileData.FileId}");
            Console.WriteLine($"Data: {fileData.Data}");
            Console.WriteLine("---------------------------------------------------");
        }

    }
}
