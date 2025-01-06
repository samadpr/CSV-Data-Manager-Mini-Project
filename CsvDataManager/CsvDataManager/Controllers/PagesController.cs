using Microsoft.AspNetCore.Mvc;

namespace CsvDataManager.Controllers
{
    public class PagesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult CsvImport()
        {
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
