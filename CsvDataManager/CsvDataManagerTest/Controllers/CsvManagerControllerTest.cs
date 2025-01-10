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

        // Test for SaveCsvUploader API
        [Fact]
        public async Task SaveCsvUploader_ShouldReturnOk_WhenValidRequestIsProvided()
        {
            // Arrange
            var request = new
            {
                FileName = "test.csv",
                UploadedBy = "testuser@example.com",
                UploadDate = "2024-01-09"
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("/csv-data-manager/save-csv-uploader", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("CsvUploader data saved successfully.", content);
        }

        [Fact]
        public async Task SaveCsvUploader_ShouldReturnBadRequest_WhenRequestIsNull()
        {
            // Arrange
            object request = null;

            // Act
            var response = await _httpClient.PostAsJsonAsync("/csv-data-manager/save-csv-uploader", request);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid data.", content);
        }

        [Fact]
        public async Task SaveCsvUploader_ShouldReturnInternalServerError_WhenUnexpectedErrorOccurs()
        {
            // Arrange
            var request = new
            {
                FileName = "test.csv",
                UploadedBy = "testuser@example.com",
                UploadDate = "2024-01-09"
            };

            // Simulate an error by sending an invalid request
            var response = await _httpClient.PostAsync("/csv-data-manager/save-csv-uploader", null);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Internal server error", content);
        }

        // Test for SaveFileDataBatch API
        [Fact]
        public async Task SaveFileDataBatch_ShouldReturnOk_WhenValidBatchDataIsProvided()
        {
            // Arrange
            var request = new
            {
                BatchId = "12345",
                FileData = new[]
                {
                    new { RowId = 1, Data = "Row 1 data" },
                    new { RowId = 2, Data = "Row 2 data" }
                }
            };

            // Act
            var response = await _httpClient.PostAsJsonAsync("/csv-data-manager/save-file-data", request);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("File data batch saved successfully.", content);
        }

        [Fact]
        public async Task SaveFileDataBatch_ShouldReturnBadRequest_WhenRequestBatchIsNull()
        {
            // Arrange
            object requestBatch = null;

            // Act
            var response = await _httpClient.PostAsJsonAsync("/csv-data-manager/save-file-data", requestBatch);

            // Assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid or empty batch data.", content);
        }

        [Fact]
        public async Task SaveFileDataBatch_ShouldReturnInternalServerError_WhenUnexpectedErrorOccurs()
        {
            // Arrange
            var request = new
            {
                BatchId = "12345",
                FileData = new[]
                {
                    new { RowId = 1, Data = "Row 1 data" },
                    new { RowId = 2, Data = "Row 2 data" }
                }
            };

            // Simulate an error by sending an invalid request
            var response = await _httpClient.PostAsync("/csv-data-manager/save-file-data", null);

            // Assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Internal server error", content);
        }
    }
}
