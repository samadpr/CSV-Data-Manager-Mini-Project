using CsvDataManager.Dtos;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;

namespace CsvDataManager.Controllers
{
    public class PublicController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _apiBaseUrl = "https://localhost:7239/api/v1";

        public PublicController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(UserDto userDto)
        {
            if (!ModelState.IsValid)
                return View();

            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync($"{_apiBaseUrl}/user/register", userDto);

            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Registration successfully!";
                return RedirectToAction("Login");
            }
            else
            {
                TempData["ErrorMessage"] = "Registration failed!";
                return View();
            }

        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.PostAsJsonAsync($"{_apiBaseUrl}/user/login", loginDto);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();

                try
                {
                    var responseObject = JObject.Parse(responseContent);
                    var token = responseObject["token"]?.ToString();
                    var userId = responseObject["userId"]?.ToString();

                    if (string.IsNullOrEmpty(token))
                    {
                        TempData["ErrorMessage"] = "Token not received from API.";
                        return View();
                    }

                    var jwtHandler = new JwtSecurityTokenHandler();
                    if (!jwtHandler.CanReadToken(token))
                    {
                        TempData["ErrorMessage"] = "Invalid token format received.";
                        return View();
                    }

                    var jwtToken = jwtHandler.ReadJwtToken(token);

                    if (jwtToken.ValidTo < DateTime.UtcNow)
                    {
                        TempData["ErrorMessage"] = "Token is expired.";
                        return View();
                    }

                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                    HttpContext.Session.SetString("Token", token);
                    HttpContext.Session.SetString("Email", loginDto.Email);
                    HttpContext.Session.SetString("UserId", userId);

                    return RedirectToAction("Index", "Pages");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error parsing token: {ex.Message}");
                    TempData["ErrorMessage"] = "An error occurred while processing the login.";
                    return View();
                }
            }
            else
            {
                var errorDetails = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Login API Error: {errorDetails}");
                TempData["ErrorMessage"] = "Login failed.";
                return View();
            }
        }


    }
}
