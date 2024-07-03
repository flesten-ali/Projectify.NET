using Microsoft.AspNetCore.Mvc;

namespace Practice.Controllers
{
    public class MusicStoreController : Controller
    {
    

        public IActionResult Index()
        {

            return View();
        }
    }
}
