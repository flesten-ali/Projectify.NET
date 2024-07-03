using Microsoft.AspNetCore.Mvc;

namespace Practice.Controllers
{
    public class tryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
