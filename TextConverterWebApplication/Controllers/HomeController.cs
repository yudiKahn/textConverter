using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TextConverterLibrary;
using TextConverterWebApplication.Models;

namespace TextConverterWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Index(Input input)
        {
            var converter = new TextConverterLibrary.Converter();
            var output = converter.Convert(input.InputFormat, input.OutputFormat, input.Text);
            ViewData["output"] = output;
            return View();
        }
        public IActionResult Index()
        {
            ViewData["output"] = "";
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}