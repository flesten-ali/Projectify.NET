using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practice.Db;
using Practice.Models;

namespace Practice.Controllers
{
    public class AddCategoriesController : Controller
    {
        private readonly AuthDbContext _context;

        public AddCategoriesController(AuthDbContext context)
        {
            _context = context;
        }

        // GET: AddCategories
        public async Task<IActionResult> Index()
        {
              return View(await _context.categories.ToListAsync());
        }

        // GET: AddCategories/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.categories == null)
            {
                return NotFound();
            }

            var category = await _context.categories
                .FirstOrDefaultAsync(m => m.categoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: AddCategories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: AddCategories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( string category)
        {
            if (category!=null &&ModelState.IsValid)
            {
                var categorry = new Category
                {
                    category = category
                };
                _context.Add(categorry);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            if(category == null)
            {
                ModelState.AddModelError("category", "Please Enter CateoryName");
            }
            return View(category);
        }

        // GET: AddCategories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.categories == null)
            {
                return NotFound();
            }

            var category = await _context.categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: AddCategories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Category categ)
        {
            if (id != categ.categoryId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(categ);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(categ.categoryId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(categ);
        }

        // GET: AddCategories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.categories == null)
            {
                return NotFound();
            }

            var category = await _context.categories
                .FirstOrDefaultAsync(m => m.categoryId == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: AddCategories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.categories == null)
            {
                return Problem("Entity set 'AuthDbContext.categories'  is null.");
            }

            var category = await _context.categories.FindAsync(id);
            if (category != null)
            {
                var projectsToDelete = _context.projects.Where(p => p.categoryId == id);
                _context.projects.RemoveRange(projectsToDelete);


                _context.categories.Remove(category);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
          return _context.categories.Any(e => e.categoryId == id);
        }
    }
}
