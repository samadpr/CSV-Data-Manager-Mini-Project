using CsvDataManager.Dtos;
using Microsoft.AspNetCore.Mvc;

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
    }
}
