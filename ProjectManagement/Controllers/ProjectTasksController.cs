using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.Models;

namespace ProjectManagement.Controllers
{
    public class ProjectTasksController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _user;

        public ProjectTasksController(AppDbContext context, UserManager<User> user)
        {
            _context = context;
            _user = user;   
        }

        // GET: ProjectTasks
        [Authorize]
        public async Task<IActionResult> Index()
        {
            string userId = _user.GetUserId(HttpContext.User);
            var results = _context.Tasks.Where(x => x.UserId == userId).Include(p => p.Project).Include(u => u.User).ToList();
            return View(results);
        }

        // GET: ProjectTasks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var projectTask = await _context.Tasks
                .Include(p => p.Project)
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (projectTask == null)
            {
                return NotFound();
            }

            return View(projectTask);
        }

        // GET: ProjectTasks/Create
        public IActionResult Create()
        {
            ViewData["UserId"]= new SelectList(_context.Users, "Id", "UserName");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Name");
            return View();
        }

        // POST: ProjectTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskId,Name,Description,Time,StartDate,EndDate,ProjectId,UserId")] ProjectTask projectTask)
        {
            var user = _context.Users.FirstOrDefault(x => x.Id == projectTask.UserId);
            user.TotalTime += projectTask.Time;
            _context.Add(projectTask);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Projects", new { id = projectTask.ProjectId });
        }

        // GET: ProjectTasks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var projectTask = await _context.Tasks.FindAsync(id);
            if (projectTask == null)
            {
                return NotFound();
            }
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Name", projectTask.ProjectId);
            return View(projectTask);
        }

        // POST: ProjectTasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TaskId,Name,Description,StartDate,EndDate,ProjectId,UserId")] ProjectTask projectTask)
        {
            if (id != projectTask.TaskId)
            {
                return NotFound();
            }

            try
            {
                _context.Update(projectTask);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectTaskExists(projectTask.TaskId))
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

        // GET: ProjectTasks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Tasks == null)
            {
                return NotFound();
            }

            var projectTask = await _context.Tasks
                .Include(p => p.Project)
                .FirstOrDefaultAsync(m => m.TaskId == id);
            if (projectTask == null)
            {
                return NotFound();
            }

            return View(projectTask);
        }

        // POST: ProjectTasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tasks == null)
            {
                return Problem("Entity set 'AppDbContext.Tasks'  is null.");
            }
            var projectTask = await _context.Tasks.FindAsync(id);
            if (projectTask != null)
            {
                _context.Tasks.Remove(projectTask);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectTaskExists(int id)
        {
          return (_context.Tasks?.Any(e => e.TaskId == id)).GetValueOrDefault();
        }
    }
}
