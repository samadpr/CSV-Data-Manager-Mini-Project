using CsvDataManagerTest.Fixtures;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Xunit;

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
        public async Task SaveCsvUploader_ShouldReturnOk_WhenValidRequestIsProvided()
        {
            var request = new
            {
                FileName = "test.csv",
                UploadedBy = "testuser@example.com",
                UploadDate = "2024-01-09"
            };

            var response = await _httpClient.PostAsJsonAsync("/csv-data-manager/save-csv-uploader", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("CsvUploader data saved successfully.", content);
        }

        [Fact]
        public async Task SaveCsvUploader_ShouldReturnBadRequest_WhenRequestIsNull()
        {
            object request = null;

            var response = await _httpClient.PostAsJsonAsync("/csv-data-manager/save-csv-uploader", request);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid data.", content);
        }

        [Fact]
        public async Task SaveCsvUploader_ShouldReturnInternalServerError_WhenUnexpectedErrorOccurs()
        {
            var request = new
            {
                FileName = "test.csv",
                UploadedBy = "testuser@example.com",
                UploadDate = "2024-01-09"
            };

            var response = await _httpClient.PostAsync("/csv-data-manager/save-csv-uploader", null);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Internal server error", content);
        }

        [Fact]
        public async Task SaveFileDataBatch_ShouldReturnOk_WhenValidBatchDataIsProvided()
        {
            var request = new
            {
                BatchId = "12345",
                FileData = new[]
                {
                    new { RowId = 1, Data = "Row 1 data" },
                    new { RowId = 2, Data = "Row 2 data" }
                }
            };

            var response = await _httpClient.PostAsJsonAsync("/csv-data-manager/save-file-data", request);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("File data batch saved successfully.", content);
        }

        [Fact]
        public async Task SaveFileDataBatch_ShouldReturnBadRequest_WhenRequestBatchIsNull()
        {
            object requestBatch = null;

            var response = await _httpClient.PostAsJsonAsync("/csv-data-manager/save-file-data", requestBatch);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid or empty batch data.", content);
        }

        [Fact]
        public async Task SaveFileDataBatch_ShouldReturnInternalServerError_WhenUnexpectedErrorOccurs()
        {
            var request = new
            {
                BatchId = "12345",
                FileData = new[]
                {
                    new { RowId = 1, Data = "Row 1 data" },
                    new { RowId = 2, Data = "Row 2 data" }
                }
            };

            var response = await _httpClient.PostAsync("/csv-data-manager/save-file-data", null);

            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Internal server error", content);
        }
    }
}
