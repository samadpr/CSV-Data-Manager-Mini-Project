using CsvDataManager.Dtos;
using CsvDataManager.Service;
using CsvDataManager.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace CsvDataManager.Controllers
{
    public class PagesController : Controller
    {
        private readonly FileProcessingService _fileProcessingService;
        private readonly ICsvDataRetrieveApiService _csvDataRetrieveApiService;

        public PagesController(FileProcessingService fileProcessingService, ICsvDataRetrieveApiService csvDataRetrieveApiService)
        {
            _fileProcessingService = fileProcessingService;
            _csvDataRetrieveApiService = csvDataRetrieveApiService;
        }
        public async Task<IActionResult> Index()
        {
            string userIdString = HttpContext.Session.GetString("UserId");
            if (string.IsNullOrEmpty(userIdString))
            {
                ViewData["Message"] = "❌ User session not found.";
                return View(new List<CsvFileModelDto>());
            }

            Guid userId = Guid.Parse(userIdString);
            var csvFileList = await _csvDataRetrieveApiService.GetUploadedCsvFilesByUserIdAsync(userId);

            return View(csvFileList);
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

            var fileDataList = await _csvDataRetrieveApiService.GetFileDataByUserIdAsync(userId);

            var parsedData = fileDataList
                .Select(fd => JsonConvert.DeserializeObject<Dictionary<string, string>>(fd["data"]))
                .ToList();

            return View(parsedData);
        }

        public IActionResult ListData()
        {
            return View();
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
