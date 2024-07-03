using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Practice.Db;
using Practice.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;

namespace Practice.Controllers
{
    [Authorize ]
    public class ProjectsController : Controller
    {
       

        private readonly AuthDbContext _context;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<IdentityUser> _userManager;

        //public ProjectsController(AuthDbContext context)
        //{
        //    _context = context;
        //}


        public ProjectsController(AuthDbContext context, IWebHostEnvironment hostingEnvironment, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
             
            
            var curUser = await _userManager.GetUserAsync(User); 
            if(curUser == null )
            {
                return NotFound();  
            }
            //if (TempData["dateOfPublish"] != null)
            //{
            //    ViewBag.DateOfPublish = TempData["dateOfPublish"];
            //}
            var userProject = await _context.projects.Include(p => p.User).Include(p=>p.Category).Where(p=>p.userId == curUser.Id).ToListAsync();
            return View(userProject);
            //return PartialView("_listProjects", userProject);

        }

        // GET: Projects/Details/5
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

        // GET: Projects/Create
        public async Task<IActionResult> Create()
        {
            //string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);  
            var currUser = await _userManager.GetUserAsync(User);
            var userId = currUser.Id;
            if (userId == null  )
            {
                return NotFound();
            }
            var categories = _context.categories.ToList();
            //collect list of catigories and send  it the view 
            ViewBag.categorylist = new SelectList(_context.categories, "categoryId", "category");
            ViewBag.userId = userId;
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Project project)
        {
            if (ModelState.IsValid)
            {

                if (project.PDFfile != null && project.PDFfile.Length > 0)
                {
                    if (Path.GetExtension(project.PDFfile.FileName).ToLower() != ".pdf")
                    {

                        ModelState.AddModelError("PDFfile", "File must be a PDF");
                        ViewBag.categorylist = new SelectList(_context.categories, "categoryId", "category");

                        return View(project);
                    }
                    string folder = "PDF/";
                    project.pdfURL = await UploadFile(folder, project.PDFfile);

                }

                 
                var pro = new Project
                {
                    categoryId = project.categoryId,
                    Title = project.Title,
                    Discreption = project.Discreption,
                    pdfURL = project.pdfURL,
                    userId = project.userId,
                 };

                project.DateOfPublish = DateTime.Now;

                _context.Add(project);
                await _context.SaveChangesAsync();
                //TempData["dateOfPublish"] = dateOnly.ToString("dd/MM/yyyy");
                return RedirectToAction("Index" , "try");
            }

            ViewBag.categorylist = new SelectList(_context.categories, "categoryId", "category");
            return   View(project);
        }
     

        ///upload 
        ///
        public async Task<string> UploadFile(string folder, IFormFile file)
        {
             

            string fileName = Guid.NewGuid().ToString() + "_" + Path.GetFileName(file.FileName);

            // Combine folder and file name to get the relative file path
            string relativeFilePath = Path.Combine(folder, fileName);

            // Combine file path with web root path to get full physical path
            string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, folder);
            string physicalFilePath = Path.Combine(uploadsFolder, fileName);

            // Ensure the directory exists, if not create it
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Save the file
            using (var fileStream = new FileStream(physicalFilePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }

            // Return the relative file path
            return relativeFilePath;
        }  







    
        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
              
            if (id == null || _context.projects == null)
            {
                return NotFound();
            }
            string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
            {
                return NotFound();
            }
              
             ViewBag.userId = userId;
            var project = await _context.projects.Include(p=>p.User).FirstOrDefaultAsync(p=>p.projectId==id);
              
  
            if (project == null)
            {
                return NotFound();
            }
            ViewBag.categorylist = new SelectList(_context.categories, "categoryId", "category");
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Project project)
        {
            if (id != project.projectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {


                try
                {

                    if (project.PDFfile != null && project.PDFfile.Length > 0)
                    {
                        // Delete the old PDF file if it exists 
                        if (!string.IsNullOrEmpty(project.pdfURL))
                        {
                            string oldPdfPath = Path.Combine(_hostingEnvironment.WebRootPath, project.pdfURL);
                            if (System.IO.File.Exists(oldPdfPath))
                            {
                                System.IO.File.Delete(oldPdfPath);
                            }
                            
                        }
                        if (Path.GetExtension(project.PDFfile.FileName).ToLower() != ".pdf")
                        {

                            ModelState.AddModelError("PDFfile", "File must be a PDF");
                            ViewBag.categorylist = new SelectList(_context.categories, "categoryId", "category");

                            return View(project);
                        }
                        string folder = "PDF/";
                        project.pdfURL = await UploadFile(folder, project.PDFfile);


                    }




                    //TempData["dateOfPublish"] = dateOnly.ToString("dd/MM/yyyy");
                    project.DateOfPublish = DateTime.Now;

                    ///
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.projectId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "try");

            }
            ViewBag.categorylist = new SelectList(_context.categories, "categoryId", "category");
            return View(project);
        }

        // GET: Projects/Delete/5
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

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
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
            return RedirectToAction("Index");
        }

        private bool ProjectExists(int id)
        {
          return _context.projects.Any(e => e.projectId == id);
        }


       
    }
}
