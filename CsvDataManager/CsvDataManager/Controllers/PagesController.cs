using CsvDataManager.Dtos;
using CsvDataManager.Service;
using Microsoft.AspNetCore.Mvc;

namespace CsvDataManager.Controllers
{
    public class PagesController : Controller
    {
        private readonly FileProcessingService _fileProcessingService;
        private readonly CsvDataRetrieveApiService _csvDataRetrieveApiService;

        public PagesController(FileProcessingService fileProcessingService, CsvDataRetrieveApiService csvDataRetrieveApiService)
        {
            _fileProcessingService = fileProcessingService;
            _csvDataRetrieveApiService = csvDataRetrieveApiService;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CsvImport()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CsvImport(TypeOfFileDto typeOfFileDto)
        {
            if (typeOfFileDto == null || (string.IsNullOrEmpty(typeOfFileDto.NetworkPath) && typeOfFileDto.CsvFile == null))
            {
                ViewData["Message"] = "Please provide a valid file or network path.";
                ViewData["MessageClass"] = "error";  
                return View();
            }

            try
            {
                string filePath = string.Empty;

                if (typeOfFileDto.CsvFile != null)
                {
                    filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads", typeOfFileDto.CsvFile.FileName);
                    var directoryPath = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(directoryPath))
                    {
                        Directory.CreateDirectory(directoryPath);
                    }

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await typeOfFileDto.CsvFile.CopyToAsync(fileStream);
                    }
                }
                else if (!string.IsNullOrEmpty(typeOfFileDto.NetworkPath))
                {
                    filePath = typeOfFileDto.NetworkPath;
                }

                string userIdString = HttpContext.Session.GetString("UserId");
                Guid userId = Guid.Parse(userIdString ?? Guid.Empty.ToString());

                var status = await _fileProcessingService.ProcessFileAndSendToQueue(filePath, userId);
                ViewData["Message"] = status;

                if (status.Contains("❌"))
                {
                    ViewData["MessageClass"] = "error";
                }
                else
                {
                    ViewData["MessageClass"] = "success";
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = $"❌ An unexpected error occurred: {ex.Message}";
                ViewData["MessageClass"] = "error";  
            }

            return View();

        }



        public async Task<IActionResult> ManageData()
        {
            string userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                ViewData["Message"] = "❌ User session not found.";
                return View(new List<Dictionary<string, string>>());
            }

            Guid userId = Guid.Parse(userIdString);

            // Retrieve data from the API service
            var fileDataList = await _csvDataRetrieveApiService.GetFileDataByUserIdAsync(userId);

            return View(fileDataList);
        }

        public IActionResult Profile()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Settings()
        {
            return View();
        }


    }


}
