using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace SampleApp.Controllers
{
    public class LoadController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            ViewBag.SampleText = "PrintMe Button Before Click";
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> IndexAsync()
        {
            // Simulate a delay
            //await Task.Delay(4000);
            await Task.Delay(4000);

            ViewBag.SampleText = "PrintMe Button After Click";
            return RedirectToAction("Index" ,"Home");
        }
    }
}
