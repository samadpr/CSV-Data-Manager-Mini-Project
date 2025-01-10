using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using CsvDataManagerTest.Fixtures;
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
        public async Task Register_ShouldReturnBadRequest_WhenEmailOrPasswordIsNullOrEmpty()
        {
            var signUpRequest = new { Email = "", Password = "" };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7239/api/v1/user/register", signUpRequest);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            var errorResponse = JsonConvert.DeserializeObject<ProblemDetails>(content);

            Assert.Contains("Email and Password are required", errorResponse.Detail);
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
        public async Task Register_ShouldReturnOk_WhenUserRegisteredSuccessfully()
        {
            var signUpRequest = new { Email = "test@example.com", Password = "password123" };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7239/api/v1/user/register", signUpRequest);

            Assert.Equal(System.Net.HttpStatusCode.OK, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("User registered successfully", content);
        }

        [Fact]
        public async Task Register_ShouldReturnInternalServerError_WhenUnexpectedErrorOccurs()
        {
            var signUpRequest = new { Email = "test@example.com", Password = "password123" };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7239/api/v1/user/register", signUpRequest);

            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("An unexpected error occurred while processing your request", content);
        }

        [Fact]
        public async Task Login_ShouldReturnBadRequest_WhenEmailOrPasswordIsNullOrEmpty()
        {
            var loginRequest = new { Email = "", Password = "" };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7239/api/v1/user/login", loginRequest);

            Assert.Equal(System.Net.HttpStatusCode.BadRequest, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("Email and Password are required", content);
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

        [Fact]
        public async Task Login_ShouldReturnInternalServerError_WhenUnexpectedErrorOccurs()
        {
            var loginRequest = new { Email = "test@example.com", Password = "password123" };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7239/api/v1/user/login", loginRequest);

            Assert.Equal(System.Net.HttpStatusCode.InternalServerError, response.StatusCode);
            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("An unexpected error occurred while processing your request", content);
        }
    }
}

