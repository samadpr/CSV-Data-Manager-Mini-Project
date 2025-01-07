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

        public void ProcessFileAndSendToQueue(string filePath, Guid userId)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    throw new FileNotFoundException("File not found at the specified path.");
                }

                using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = new StreamReader(stream);

                Guid sharedId = Guid.NewGuid();
                int rowCount = 0;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    var values = line?.Split(',');

                    if (values == null || values.Length == 0)
                        continue;

                    var fileData = new FileDataDto
                    {
                        FileId = sharedId,
                        Data = line,
                        Id = sharedId,
                        FileName = Path.GetFileName(filePath),
                        Extension = Path.GetExtension(filePath),
                        FilePath = filePath,
                        FileSize = new FileInfo(filePath).Length,
                        NoOfRow = ++rowCount,
                        Status = "In Progress",
                        UserId = userId
                    };

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

        private void PublishToQueue(FileDataDto fileData)
        {
            try
            {
                _channel.QueueDeclare(
                    queue: "csv-data-queue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null
                );

                var message = JsonConvert.SerializeObject(fileData);
                var body = Encoding.UTF8.GetBytes(message);

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
