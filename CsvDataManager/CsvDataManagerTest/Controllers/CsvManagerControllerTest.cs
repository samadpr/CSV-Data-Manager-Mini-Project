using CsvDataManagerTest.Fixtures;
using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using CsvManagerAPI.API.CsvDataManage.RequestObject;


namespace CsvDataManagerTest.Controllers
{
    public class CsvManagerControllerTest : IClassFixture<WebApiApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public CsvManagerControllerTest(WebApiApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task SaveCsvUploader_ShouldReturnBadRequest_WhenValidRequestIsProvided()
        {
            var request = new
            {
                 Id = Guid.NewGuid(),
                 FileName = "test.csv",
                 Extension=  "csv",
                 FilePath = "C:\\path\\to\\file.csv",
                 FileSize = "10KB",
                 NoOfRow  = "49",
                 Status   = "Pending",
                 UserId  = "1",
            };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7239/api/v1/csv-data-manager/save-csv-uploader", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains(".", content);
        }

        [Fact]
        public async Task SaveCsvUploader_ShouldReturnBadRequest_WhenRequestIsNull()
        {
            object request = null;

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7239/api/v1/csv-data-manager/save-csv-uploader", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains(content, content);
        }

        [Fact]
        public async Task SaveFileDataBatch_ShouldReturnOk_WhenValidBatchDataIsProvided()
        {
            var request = new FileDataRequestObject
            {
                FileId = Guid.NewGuid(),
                Data = "Row 1 data,Row 2 data"
            };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7239/api/v1/csv-data-manager/save-file-data", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("File data batch saved successfully.", content);
        }


        [Fact]
        public async Task SaveFileDataBatch_ShouldReturnBadRequest_WhenRequestBatchIsNull()
        {
            object requestBatch = null;

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7239/api/v1/csv-data-manager/save-file-data", requestBatch);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("One or more validation errors occurred.", content);
        }

    }
}
