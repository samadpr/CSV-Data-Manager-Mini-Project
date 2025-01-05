using Microsoft.AspNetCore.Mvc;

namespace CsvDataManager.Controllers
{
    public class PagesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
