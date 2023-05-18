using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.Models;

namespace ProjectManagement.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _user;

        public ProjectsController(AppDbContext context, UserManager<User> user)
        {
            _context = context;
            _user = user;
        }

        // GET: Projects
        [Authorize]
        public async Task<IActionResult> Index()
        {
            string userId = _user.GetUserId(HttpContext.User);
            var user = _context.Users.FirstOrDefault(U => U.Id == userId);
            var results = _context.Projects.Where(x => x.Users.Contains(user));
            return View(results);
        }

        public async Task<IActionResult> ArchivalProjects()
        {
            string userId = _user.GetUserId(HttpContext.User);
            var user = _context.Users.FirstOrDefault(U => U.Id == userId);
            var results = _context.Projects.Where(x => x.Users.Contains(user));
            return View(results);
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int id)
        {
            ViewBag.UserId = _user.GetUserId(HttpContext.User);
            
            List<User> users = _context.Users.ToList();
            ViewBag.Users = users;

            List<ProjectTask> tasks = _context.Tasks.Where(x => x.ProjectId == id).Include(p => p.User).ToList();
            ViewBag.Tasks = tasks;

            List<Risk> risks = _context.Risks.Where(x => x.ProjectId == id).ToList();
            ViewBag.Risks = risks;

            List<Goal> goals = _context.Goals.Where(x => x.ProjectId == id).ToList();
            ViewBag.Goals = goals;

            List<Milestone> milestones = _context.Milestones.Where(x => x.ProjectId == id).ToList();
            ViewBag.Milestones = milestones;

            List<Comment> comments = _context.Comments.Where(x => x.ProjectId == id).Include(p => p.User).ToList();
            comments.Reverse();
            ViewBag.Comments = comments;

            if (_context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.Include(u=>u.Users)
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }
        //Create goals
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateGoal(int projectId, string name)
        {
            var project = _context.Projects.Find(projectId);
            if (project != null)
            {
                var goal = new Goal
                {
                    ProjectId = projectId,
                    Name = name
                };

                _context.Goals.Add(goal);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Details), new { id = projectId });

        }

        //Create milestones
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMilestone(int projectId, string name, DateTime date)
        {
            var project = _context.Projects.Find(projectId);
            if (project != null)
            {
                var milestone = new Milestone
                {
                    ProjectId = projectId,
                    Name = name,
                    Date = date
                };

                _context.Milestones.Add(milestone);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Details), new { id = projectId });

        }

        //Add comments
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment(int projectId, string commentText)
        {
            var project = _context.Projects.Find(projectId);
            if (project != null)
            {
                var userId = _user.GetUserId(HttpContext.User);
                var comment = new Comment
                {
                    Project = project,
                    UserId = userId,
                    Content = commentText,
                    CreateDate = DateTime.Now
                };

                _context.Comments.Add(comment);
                _context.SaveChanges();
            }

            return RedirectToAction(nameof(Details), new { id = projectId });

        }
        //Delete comments
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteComment(int projectId, int commentId)
        {

            var comment = _context.Comments.Find(commentId);
            _context.Comments.Remove(comment);
            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = projectId });

        }
        //Add user
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddUserToProject(int projectId, string userId)
        {
            Models.Project project = _context.Projects.Include(p => p.Users).FirstOrDefault(p => p.ProjectId == projectId);
            User user = _context.Users.FirstOrDefault(u => u.Id == userId);
            project.Users.Add(user);
            _context.SaveChanges();

            return RedirectToAction(nameof(Details), new { id = projectId });
        }

        public IActionResult ChangeStatus(int id)
        {
            var project = _context.Projects.FirstOrDefault(t => t.ProjectId == id);

            if (project != null)
            {
                project.Status = !project.Status;
                _context.SaveChanges();
            }
            return RedirectToAction("Index");
        }

        // GET: Projects/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,Name,Description,StartDate,EndDate,Status,TotalBudget,ProjectScope,Sponsor,Stakeholders")] Models.Project project)
        {
            User currentUser = await _user.GetUserAsync(User);

            if (currentUser != null)
            {
                project.Users = new List<User> { currentUser };
            }
            _context.Add(project);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.FindAsync(id);
            if (project == null)
            {
                return NotFound();
            }
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,Name,Description,StartDate,EndDate,Status")] Models.Project project)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            try
            {
                _context.Update(project);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProjectExists(project.ProjectId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Details), new { id = project.ProjectId });

        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Projects == null)
            {
                return NotFound();
            }

            var project = await _context.Projects
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Projects == null)
            {
                return Problem("Entity set 'AppDbContext.Projects'  is null.");
            }
            var project = await _context.Projects.FindAsync(id);
            if (project != null)
            {
                _context.Projects.Remove(project);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return (_context.Projects?.Any(e => e.ProjectId == id)).GetValueOrDefault();
        }
    }
}
