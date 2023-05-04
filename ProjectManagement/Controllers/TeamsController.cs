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
    public class TeamsController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _user;

        public TeamsController(AppDbContext context, UserManager<User> user)
        {
            _context = context;
            _user = user;
        }

        [Authorize]
        // GET: Teams
        public async Task<IActionResult> Index()
        {
            string userId = _user.GetUserId(HttpContext.User);
            var results = _context.Teams.Where(x => x.LeaderId == userId).Include(p => p.Leader).ToList();
            return View(results);
        }

        [HttpPost]
        public async Task<IActionResult> AddMember(int TeamId, string userId)
        {
            var team = await _context.Teams.FirstOrDefaultAsync(m => m.TeamId == TeamId);
            var user = await _context.Users.FirstOrDefaultAsync(m => m.Id == userId);
            team.Users.Add(user);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: Teams/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            ViewData["Users"] = new SelectList(_context.Users, "Id", "UserName");

            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.Include(p => p.Leader)
                .FirstOrDefaultAsync(m => m.TeamId == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // GET: Teams/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Teams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeamId,Name,LeaderId")] Team team)
        {
            var userId = _user.GetUserId(HttpContext.User);
            team.LeaderId = userId;
            _context.Add(team);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Teams/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "UserName");
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }
            return View(team);
        }

        // POST: Teams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeamId,Name,LeaderId")] Team team)
        {
            if (id != team.TeamId)
            {
                return NotFound();
            }

            try
            {
                _context.Update(team);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(team.TeamId))
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

        // GET: Teams/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Teams == null)
            {
                return NotFound();
            }

            var team = await _context.Teams
                .FirstOrDefaultAsync(m => m.TeamId == id);
            if (team == null)
            {
                return NotFound();
            }

            return View(team);
        }

        // POST: Teams/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Teams == null)
            {
                return Problem("Entity set 'AppDbContext.Teams'  is null.");
            }
            var team = await _context.Teams.FindAsync(id);
            if (team != null)
            {
                _context.Teams.Remove(team);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeamExists(int id)
        {
          return (_context.Teams?.Any(e => e.TeamId == id)).GetValueOrDefault();
        }
    }
}
