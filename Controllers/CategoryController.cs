using Practice.Db;
using Practice.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Practice.Controllers
{
    public class CategoryController : Controller
    {
        private readonly AuthDbContext _context;

        public CategoryController(AuthDbContext context  )
        {
            _context = context;
         
        }
        public IActionResult Index()
        {
            var categories = _context.categories.ToList();
            //collect list of catigories and send  it the view 
            ViewBag.categorylist = new SelectList(_context.categories, "categoryId", "category");
            
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(Category categgoory)
        {
         
            if(categgoory != null) {
                var projectOfCategory = _context.projects .
                     Include(p=>p.Category).
                     Where(p => p.categoryId == categgoory.categoryId).Include(p => p.User).ToList();
                //collect list of catigories and send  it the view 
             
                return View("DisplayCategory", projectOfCategory);


            }
            ViewBag.categorylist = new SelectList(_context.categories, "categoryId", "category");

            return View(categgoory);
        }

        public IActionResult ProjectDetails(int? projectId)
        {
            if (projectId != null)
            {
                var project = _context.projects.
                                 Include(p => p.Category)
                                 .Include(p => p.User)
                                 .FirstOrDefault(p => p.projectId == projectId);
                return View(project);

            }
            return View("DisplayCategory");
        }


    }
}
