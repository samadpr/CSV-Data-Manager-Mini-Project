using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using RabbitMQ.Client;
using Newtonsoft.Json;
using QueueWorkerConsole.Dto;

namespace QueueWorkerConsole.CsvDataReceiver
{
    public static class QueueCsvDataReceiver
    {
        public static void Receiver(IModel channel)
        {
            channel.QueueDeclare(
                queue: "csv-data-queue",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null
            );

            Console.WriteLine("Waiting for messages from 'csv-data-queue'...");

            // Create a consumer to listen to the queue
            var receiver = new EventingBasicConsumer(channel);
            receiver.Received += async (model, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    Console.WriteLine($"Received message: {message}");

                    // Deserialize the message
                    var csvData = JsonConvert.DeserializeObject<CsvFileModelDto>(message);

                    if (csvData == null)
                    {
                        Console.WriteLine("Invalid message format. Skipping...");
                        channel.BasicReject(ea.DeliveryTag, false); // Reject message without requeue
                        return;
                    }

                    // Call the API to save the data
                    bool isSaved = await SendToApiAsync(csvData);

                    if (isSaved)
                    {
                        // Acknowledge the message if saved successfully
                        channel.BasicAck(ea.DeliveryTag, false);
                        Console.WriteLine("Message acknowledged and saved to the API.");
                    }
                    else
                    {
                        Console.WriteLine("Failed to save the message. Retrying...");
                        // Optionally requeue the message
                        channel.BasicReject(ea.DeliveryTag, true);
                    }
                }
                catch (JsonException jsonEx)
                {
                    Console.WriteLine($"JSON deserialization error: {jsonEx.Message}");
                    channel.BasicReject(ea.DeliveryTag, false); // Reject and don't requeue
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error processing message: {ex.Message}");
                    // Reject the message and don't requeue
                    channel.BasicReject(ea.DeliveryTag, false);
                }
            };

            // Start consuming messages
            channel.BasicConsume(
                queue: "csv-data-queue",
                autoAck: false,
                consumer: receiver
            );

            Console.WriteLine("If You Press [Enter] to exit.");
            Console.ReadLine();
        }

        private static async Task<bool> SendToApiAsync(CsvFileModelDto fileData)
        {
            using var httpClient = new HttpClient();
            int retryCount = 3;

            try
            {
                for (int i = 0; i < retryCount; i++)
                {
                    var jsonContent = JsonConvert.SerializeObject(fileData);
                    var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync("https://localhost:7239/api/v1/csv-data-manager/save-csv-data", content);

                    if (response.IsSuccessStatusCode)
                    {
                        Console.WriteLine("Data saved to API successfully.");
                        return true;
                    }

                    Console.WriteLine($"API responded with: {response.StatusCode}. Retrying ({i + 1}/{retryCount})...");
                    await Task.Delay(2000); 
                }
            }
            catch (HttpRequestException httpEx)
            {
                Console.WriteLine($"HTTP error: {httpEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error while calling API: {ex.Message}");
            }

            return false;
        }
    }
}
