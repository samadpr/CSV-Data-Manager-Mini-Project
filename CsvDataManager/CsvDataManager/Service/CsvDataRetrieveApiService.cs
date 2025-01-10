using Newtonsoft.Json;

namespace CsvDataManager.Service
{
    public class CsvDataRetrieveApiService
    {
        private readonly HttpClient _httpClient;

        public CsvDataRetrieveApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Dictionary<string, string>>> GetFileDataByUserIdAsync(Guid userId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7239/api/v1/get-file-data/{userId}");
            response.EnsureSuccessStatusCode();

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var fileDataList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonResponse);

            return fileDataList ?? new List<Dictionary<string, string>>();
        }
    }
}
