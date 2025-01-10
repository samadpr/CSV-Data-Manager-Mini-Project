using CsvDataManager.Dtos;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.IO;
using System.Reflection.PortableExecutable;
using System.Text;

namespace CsvDataManager.Service
{
    public class FileProcessingService
    {
        private readonly IModel _channel;
        private CsvDataSaveApiService _csvDataSaveApiService;

        public FileProcessingService(IModel channel, CsvDataSaveApiService csvDataSaveApiService)
        {
            _channel = channel ?? throw new ArgumentNullException(nameof(channel));
            _csvDataSaveApiService = csvDataSaveApiService;
        }
        

        public async Task<string> ProcessFileAndSendToQueue(string filePath, Guid userId)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                {
                    return "❌ File not found at the specified path.";
                }

                using var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                using var reader = new StreamReader(stream);

                Guid sharedId = Guid.NewGuid();
                int rowCount = 0;

                var headersLine = await reader.ReadLineAsync();
                if (string.IsNullOrEmpty(headersLine))
                {
                    return "❌ The file is empty.";
                }

                var headers = headersLine.Split(',');

                CsvFileModelDto csvFileModelDto = new CsvFileModelDto()
                {
                    Id = sharedId,
                    FileName = Path.GetFileName(filePath),
                    Extension = Path.GetExtension(filePath),
                    FilePath = filePath,
                    FileSize = new FileInfo(filePath).Length,
                    NoOfRow = File.ReadLines(filePath).Count(),
                    Status = "Progress",
                    UserId = userId
                };

                var result = await _csvDataSaveApiService.SaveCsvDataAsync(csvFileModelDto);
                if (result != null)
                {
                    while (!reader.EndOfStream)
                    {
                        var line = await reader.ReadLineAsync();
                        var values = line?.Split(',');

                        if (values == null || values.Length == 0)
                            continue;

                        var rowData = headers.ToArray().Zip(values.ToArray(), (header, value) => new { header, value })
                                        .ToDictionary(x => x.header, x => x.value);

                        var fileData = new FileDataDto
                        {
                            FileId = sharedId,
                            Data = JsonConvert.SerializeObject(rowData)
                        };

                        rowCount++;

                        PublishToQueue(fileData);
                    }

                    return $"✅ File processed successfully. Total rows processed: {rowCount} |" + " You Can go to Manage Data page to check the data.";
                }

                return "❌ An error occurred while processing the file.";
            }
            catch (FileNotFoundException ex)
            {
                return $"❌ File error: {ex.Message}";
            }
            catch (IOException ex)
            {
                return $"❌ File access error: {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"❌ An unexpected error occurred: {ex.Message}";
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
