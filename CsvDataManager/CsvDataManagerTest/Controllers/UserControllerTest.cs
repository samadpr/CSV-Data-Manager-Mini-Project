using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using CsvDataManagerTest.Fixtures;

namespace CsvDataManagerTest.Controllers
{
    public class UserControllerTest : IClassFixture<WebApiApplicationFactory>
    {
        private readonly HttpClient _httpClient;

        public UserControllerTest(WebApiApplicationFactory factory)
        {
            _httpClient = factory.CreateClient();
        }

        [Fact]
        public async Task Register_ShouldReturnBadRequest_WhenEmailOrPasswordIsNullOrEmpty()
        {
            // Arrange
            var signUpRequest = new { Email = "", Password = "" };

            // Act
            var response = await _httpClient.PostAsJsonAsync("/user/register", signUpRequest);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Email and Password are required", content);
        }

        [Fact]
        public async Task Register_ShouldReturnConflict_WhenEmailAlreadyExists()
        {
            // Arrange
            var signUpRequest = new { Email = "test@example.com", Password = "password123" };

            // Mock the service to return a conflict
            var response = await _httpClient.PostAsJsonAsync("/user/register", signUpRequest);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Conflict, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Email already exists", content);
        }

        [Fact]
        public async Task Register_ShouldReturnOk_WhenUserRegisteredSuccessfully()
        {
            // Arrange
            var signUpRequest = new { Email = "test@example.com", Password = "password123" };

            // Mock the service to return success
            var response = await _httpClient.PostAsJsonAsync("/user/register", signUpRequest);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("User registered successfully", content);
        }

        [Fact]
        public async Task Register_ShouldReturnInternalServerError_WhenUnexpectedErrorOccurs()
        {
            // Arrange
            var signUpRequest = new { Email = "test@example.com", Password = "password123" };

            // Simulate an internal server error
            var response = await _httpClient.PostAsJsonAsync("/user/register", signUpRequest);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("An unexpected error occurred while processing your request", content);
        }

        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenEmailOrPasswordIsNullOrEmpty()
        {
            // Arrange
            var loginRequest = new { Email = "", Password = "" };

            // Act
            var response = await _httpClient.PostAsJsonAsync("/user/login", loginRequest);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Email and Password are required", content);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenInvalidCredentialsAreProvided()
        {
            // Arrange
            var loginRequest = new { Email = "invalid@example.com", Password = "wrongpassword" };

            // Act
            var response = await _httpClient.PostAsJsonAsync("/user/login", loginRequest);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid email or password", content);
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenValidCredentialsAreProvided()
        {
            // Arrange
            var loginRequest = new { Email = "test@example.com", Password = "password123" };

            // Act
            var response = await _httpClient.PostAsJsonAsync("/user/login", loginRequest);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
        }

        [Fact]
        public async Task Login_ShouldReturnInternalServerError_WhenUnexpectedErrorOccurs()
        {
            // Arrange
            var loginRequest = new { Email = "test@example.com", Password = "password123" };

            // Simulate an internal server error
            var response = await _httpClient.PostAsJsonAsync("/user/login", loginRequest);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("An unexpected error occurred while processing your request", content);
        }
    }
}

