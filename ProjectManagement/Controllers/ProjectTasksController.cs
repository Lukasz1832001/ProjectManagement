using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.Evaluation;
using Microsoft.CodeAnalysis;
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
        public IActionResult ChangeStatus(int id)
        {
            var task = _context.Tasks.FirstOrDefault(t => t.TaskId == id);

            if (task != null)
            {
                task.Status = !task.Status;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }
        // GET: ProjectTasks/Create
        public IActionResult Create(int projectId, string projectName)
        {
            var project = _context.Projects.Include(p => p.ProjectUsers)
                               .ThenInclude(pu => pu.User)
                               .FirstOrDefault(x => x.ProjectId == projectId);
            var users = project.ProjectUsers.Select(pu => pu.User).ToList();

            ViewData["Users"] = new SelectList(users, "Id", "UserName");
            ViewData["ProjectId"] = projectId;
            ViewData["ProjectName"] = projectName;
            return View();
        }

        // POST: ProjectTasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TaskId,Name,Description,Time,StartDate,EndDate,ProjectId,UserId")] ProjectTask projectTask)
        {
            projectTask.Status = false;
            var user = _context.Users.FirstOrDefault(x => x.Id == projectTask.UserId);
            user.TotalTime += projectTask.Time;
            _context.Add(projectTask);
            await _context.SaveChangesAsync();
            return RedirectToAction("Details", "Projects", new { id = projectTask.ProjectId });
        }

        // GET: ProjectTasks/Edit/5
        public async Task<IActionResult> Edit(int id)
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
            var project = _context.Projects.Include(p => p.ProjectUsers)
                               .ThenInclude(pu => pu.User)
                               .FirstOrDefault(p => p.Tasks.Contains(projectTask));

            ViewData["ProjectName"] = project.Name.ToString();
            ViewData["ProjectId"] = project.ProjectId;
            var users = project.ProjectUsers.Select(pu => pu.User).ToList();

            ViewData["Users"] = new SelectList(users, "Id", "UserName");
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
            return RedirectToAction("Details", "Projects", new { id = projectTask.ProjectId });

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
            ViewData["ProjectId"] = projectTask.ProjectId;
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
            return RedirectToAction("Details", "Projects", new { id = projectTask.ProjectId });
        }

        private bool ProjectTaskExists(int id)
        {
          return (_context.Tasks?.Any(e => e.TaskId == id)).GetValueOrDefault();
        }
    }
}
