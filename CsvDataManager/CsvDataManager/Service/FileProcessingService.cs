using CsvDataManager.Dtos;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.IO;
using System.Text;

namespace CsvDataManager.Service
{
    public class FileProcessingService
    {
        private readonly IModel _channel;

        public FileProcessingService(IModel channel)
        {
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));
        }

        public void ProcessFileAndSendToQueue(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    throw new FileNotFoundException("File not found at the specified path.");
                }

                // Use FileStream with FileShare.ReadWrite to avoid file lock issues
                using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = new StreamReader(stream);

                int rowCount = 0;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line?.Split(',');

                    // Skip invalid lines or empty rows
                    if (values == null || values.Length == 0)
                        continue;

                    // Map CSV line to your data model
                    var fileData = new CsvFileModelDto
                    {
                        Id = Guid.NewGuid(), // Generate a unique ID for each record
                        FileName = Path.GetFileName(filePath),
                        Extension = Path.GetExtension(filePath),
                        FilePath = filePath,
                        FileSize = new FileInfo(filePath).Length,
                        NoOfRow = ++rowCount,
                        Status = "In Progress",
                        UserId = Guid.NewGuid()
                    };

                    // Publish each row to RabbitMQ
                    PublishToQueue(fileData);
                }

                Console.WriteLine($"File processed successfully. Total rows processed: {rowCount}");
            }
            catch (IOException ex)
            {
                Console.WriteLine($"File access error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            }
        }

        private void PublishToQueue(CsvFileModelDto fileData)
        {
            try
            {
                // Declare the queue (if not already declared)
                _channel.QueueDeclare(
                    queue: "csv-data-queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                // Convert fileData object to JSON
                var message = JsonConvert.SerializeObject(fileData);
                var body = Encoding.UTF8.GetBytes(message);

                // Publish message to RabbitMQ
                _channel.BasicPublish(
                    exchange: "",
                    routingKey: "csv-data-queue",
                    basicProperties: null,
                    body: body
                );

                Console.WriteLine($"Message sent: {message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to publish message to queue: {ex.Message}");
            }
        }
    }
}
