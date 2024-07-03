using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practice.Db;
using Practice.Models;
using Microsoft.AspNetCore.Authorization;
 namespace Practice.Controllers
{
    [Authorize(Roles = "SuperAdmin")]

    public class UsersController : Controller
    {
        private readonly AuthDbContext _context;

        public UsersController(AuthDbContext context)
        {
            _context = context;
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            var authDbContext = _context.projects.Include(p => p.Category).Include(p => p.User);
            return View(await authDbContext.ToListAsync());
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.projects == null)
            {
                return NotFound();
            }

            var project = await _context.projects
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.projectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.projects == null)
            {
                return NotFound();
            }

            var project = await _context.projects
                .Include(p => p.Category)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.projectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.projects == null)
            {
                return Problem("Entity set 'AuthDbContext.projects'  is null.");
            }
            var project = await _context.projects.FindAsync(id);
            if (project != null)
            {
                _context.projects.Remove(project);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
          return _context.projects.Any(e => e.projectId == id);
        }
    }
}
