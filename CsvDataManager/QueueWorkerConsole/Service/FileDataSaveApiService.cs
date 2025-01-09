
using QueueWorkerConsole.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace QueueWorkerConsole.Service
{
    public class FileDataSaveApiService
    {

        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl = "https://localhost:7239/api/v1";

        public FileDataSaveApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<bool> SaveCsvDataAsync(FileDataDto fileData)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();
                var response = await client.PostAsJsonAsync($"{_apiBaseUrl}/csv-data-manager/save-file-data", fileData);

                Console.WriteLine($"Response status code: {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    Console.WriteLine(await response.Content.ReadAsStringAsync());
                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending file data to API: {ex.Message}");
            }

            return false;
        }
    }
}
