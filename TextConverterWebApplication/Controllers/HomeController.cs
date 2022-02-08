using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TextConverterLibrary;
using TextConverterWebApplication.Models;

namespace TextConverterWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private static string inputTxt = "";

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Index(Input input)
        {
            var converter = new Converter();
            var output = "";

            try
            {
                if (input.IsAuto)
                {
                    int n = new Random().Next(1, 14);
                    input.Text = Generator.Generate.Text(
                        Generator.Generate.AbstractSyntaxTree<Person>(n), input.InputFormat
                    );
                }

                output = converter.Convert(input.InputFormat, input.OutputFormat, input.Text);
            }
            catch (Exception ex)
            {
                output = $"An error accourd. {ex.Message}";
            }

            ViewData["output"] = output;
            inputTxt = input.Text.Trim();
            ViewData["input"] = inputTxt;

            return View();
        }
        public IActionResult Index()
        {
            ViewData["output"] = "";
            ViewData["input"] = inputTxt;
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