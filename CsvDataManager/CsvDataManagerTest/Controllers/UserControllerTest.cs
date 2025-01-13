using System.Net;
using System.Net.Http.Json;
using CsvDataManagerTest.Fixtures;
using CsvManagerAPI.API.User.RequestObject;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public async Task Register_ShouldReturnOk_WhenUserRegisteredSuccessfully()
        {
            // Arrange
            var signUpRequest = new SignUpRequestObject { Email = "newuser@example.com", Password = "password123" };

            // Act
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7239/api/v1/user/register", signUpRequest);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("User registered successfully", content);
        }


        [Fact]
        public async Task Register_ShouldReturnConflict_WhenEmailAlreadyExists()
        {
            var signUpRequest = new { Email = "test@example.com", Password = "password123" };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7239/api/v1/user/register", signUpRequest);

            Assert.Equal(System.Net.HttpStatusCode.Conflict, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Email already exists", content);
        }



        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenEmailOrPasswordIsNullOrEmpty()
        {
            // Arrange
            var loginRequest = new { Email = "", Password = "" };

            // Act
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7239/api/v1/user/login", loginRequest);

            // Assert
            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

            // Deserialize the response content
            var content = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ProblemDetails>(content);

            // Assert that the error message is what we expect
            Assert.NotNull(errorResponse);
            Assert.Equal("One or more validation errors occurred.", errorResponse.Title);
        }


        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenInvalidCredentialsAreProvided()
        {
            var loginRequest = new { Email = "invalid@example.com", Password = "wrongpassword" };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7239/api/v1/user/login", loginRequest);

            Assert.Equal(System.Net.HttpStatusCode.Unauthorized, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Invalid email or password", content);
        }

        [Fact]
        public async Task Login_ShouldReturnOk_WhenValidCredentialsAreProvided()
        {
            var loginRequest = new { Email = "test@example.com", Password = "password123" };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7239/api/v1/user/login", loginRequest);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.NotNull(content);
        }

       
    }
}

