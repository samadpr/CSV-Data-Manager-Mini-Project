using CsvDataManager.Dtos;
using CsvDataManager.Service.Interface;
using Newtonsoft.Json;

namespace CsvDataManager.Service
{
    public class CsvDataRetrieveApiService : ICsvDataRetrieveApiService
    {
        private readonly HttpClient _httpClient;

        public CsvDataRetrieveApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Dictionary<string, string>>> GetFileDataByUserIdAsync(Guid userId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7239/api/v1/csv-data-manager/get-file-data/{userId}");
            if(!response.IsSuccessStatusCode)
            {
                return new List<Dictionary<string, string>>();
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var fileDataList = JsonConvert.DeserializeObject<List<Dictionary<string, string>>>(jsonResponse);

            return fileDataList ?? new List<Dictionary<string, string>>();
        }


        public async Task<List<CsvFileModelDto>> GetUploadedCsvFilesByUserIdAsync(Guid userId)
        {
            var response = await _httpClient.GetAsync($"https://localhost:7239/api/v1/csv-data-manager/get-csv-file/{userId}");
            if (!response.IsSuccessStatusCode)
            {
                return new List<CsvFileModelDto>();
            }

            var jsonResponse = await response.Content.ReadAsStringAsync();
            var fileList = JsonConvert.DeserializeObject<List<CsvFileModelDto>>(jsonResponse);

            return fileList ?? new List<CsvFileModelDto>();
        }
    }
}
