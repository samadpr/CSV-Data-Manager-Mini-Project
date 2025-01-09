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
        /*private static readonly HttpClient _httpClient = new HttpClient(new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
        })
        {
            Timeout = TimeSpan.FromMinutes(5)
        };*/

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

                    /*if (!await CsvUploaderDataInsert(csvData))
                    {
                        Console.WriteLine("Failed to save CsvUploaderDto. Rejecting message.");
                        channel.BasicReject(ea.DeliveryTag, false);
                        return;
                    }*/

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

        /*private async static Task<bool> CsvUploaderDataInsert(CsvFileModelDto csvData)
        {
            var csvUploaderDto = new CsvUploaderDto
            {
                Id = csvData.Id,
                FileName = csvData.FileName,
                Extension = csvData.Extension,
                FilePath = csvData.FilePath,
                FileSize = csvData.FileSize,
                NoOfRow = csvData.NoOfRow,
                Status = csvData.Status,
                UserId = csvData.UserId
            };

            PrintCsvUploader(csvUploaderDto);

            return await SendUploaderToApiAsync(csvUploaderDto);
        }*/

        /*private static void PrintCsvUploader(CsvUploaderDto uploader)
        {
            Console.WriteLine("Received CsvUploaderDto:");
            Console.WriteLine($"Id: {uploader.Id}");
            Console.WriteLine($"FileName: {uploader.FileName}");
            Console.WriteLine($"Extension: {uploader.Extension}");
            Console.WriteLine($"FilePath: {uploader.FilePath}");
            Console.WriteLine($"FileSize: {uploader.FileSize} bytes");
            Console.WriteLine($"NoOfRow: {uploader.NoOfRow}");
            Console.WriteLine($"Status: {uploader.Status}");
            Console.WriteLine($"UserId: {uploader.UserId}");
            Console.WriteLine("---------------------------------------------------");
        }*/



        /*private static async Task<bool> SendUploaderToApiAsync(CsvUploaderDto uploader)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7239/api/v1/csv-data-manager/save-csv-uploader", uploader);
                Console.WriteLine($"Response status code: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(" CsvUploaderDto saved successfully.");
                    return true;
                }
                else
                {
                    Console.WriteLine($" API responded with: {response.StatusCode}");
                    Console.WriteLine($"Error: {await response.Content.ReadAsStringAsync()}");

                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine($" Error sending CsvUploaderDto to API: {ex.Message}");
            }

            return false;
        }*/

        /*private static async Task<bool> SendFileDataToApiAsync(FileDataDto fileData)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7239/api/v1/csv-data-manager/save-file-data", fileData);

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(" File data saved successfully.");
                    return true;
                }

                Console.WriteLine($"API responded with: {response.StatusCode}");
                Console.WriteLine($"Error: {await response.Content.ReadAsStringAsync()}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending file data to API: {ex.Message}");
            }

            return false;
        }*/
    }
}
