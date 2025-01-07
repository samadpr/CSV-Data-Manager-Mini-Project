using CsvDataManager.Dtos;
using CsvDataManager.Service;
using Microsoft.AspNetCore.Mvc;

namespace CsvDataManager.Controllers
{
    public class PagesController : Controller
    {
        private readonly FileProcessingService _fileProcessingService;

        public PagesController(FileProcessingService fileProcessingService)
        {
            _fileProcessingService = fileProcessingService;
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

                _fileProcessingService.ProcessFileAndSendToQueue(filePath);
                ViewData["Message"] = "File processed successfully!";
            }
            catch (Exception ex)
            {
                ViewData["Message"] = $"Error processing file: {ex.Message}";
            }

            return View();
        }



        public IActionResult ManageData()
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
