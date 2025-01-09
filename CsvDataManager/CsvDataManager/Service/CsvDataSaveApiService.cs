using CsvDataManager.Dtos;

namespace CsvDataManager.Service
{
    public class CsvDataSaveApiService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl = "https://localhost:7239/api/v1";

        public CsvDataSaveApiService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> SaveCsvDataAsync(CsvFileModelDto csvFileModel)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync($"{_apiBaseUrl}/csv-data-manager/save-csv-uploader", csvFileModel);
            Console.WriteLine($"Response status code: {response.StatusCode}");
            if(response.IsSuccessStatusCode)
            {
                return await response.Content.ReadAsStringAsync();
            }

            return null;
        }
    }
}
