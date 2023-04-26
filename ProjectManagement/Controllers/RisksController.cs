using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProjectManagement.Data;
using ProjectManagement.Data.Enums;
using ProjectManagement.Models;

namespace ProjectManagement.Controllers
{
    public class RisksController : Controller
    {
        private readonly AppDbContext _context;

        public RisksController(AppDbContext context)
        {
            _context = context;
            
        }

        // GET: Risks
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.Risks.Include(r => r.Project);
            return View(await appDbContext.ToListAsync());
        }

        // GET: Risks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Risks == null)
            {
                return NotFound();
            }

            var risk = await _context.Risks
                .Include(r => r.Project)
                .FirstOrDefaultAsync(m => m.RiskId == id);
            if (risk == null)
            {
                return NotFound();
            }

            return View(risk);
        }

        // GET: Risks/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Name");
            ViewData["ImpactCategories"] = new SelectList(Enum.GetValues(typeof(ImpactCategories))
                                                 .Cast<ImpactCategories>()
                                                 .Select(v => v.ToString())
                                                 .ToList());
            return View();
        }

        // POST: Risks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RiskId,Name,Description,Probability,Influence,Reaction,ProjectId")] Risk risk)
        {
            _context.Add(risk);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));

        }

        // GET: Risks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Risks == null)
            {
                return NotFound();
            }

            var risk = await _context.Risks.FindAsync(id);
            if (risk == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "ProjectId", "Name", risk.ProjectId);
            ViewData["ImpactCategories"] = new SelectList(Enum.GetValues(typeof(ImpactCategories))
                                                 .Cast<ImpactCategories>()
                                                 .Select(v => v.ToString())
                                                 .ToList());
            return View(risk);
        }

        // POST: Risks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RiskId,Name,Description,Probability,Influence,Reaction,ProjectId")] Risk risk)
        {
            if (id != risk.RiskId)
            {
                return NotFound();
            }

            try
            {
                _context.Update(risk);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RiskExists(risk.RiskId))
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

        // GET: Risks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Risks == null)
            {
                return NotFound();
            }

            var risk = await _context.Risks
                .Include(r => r.Project)
                .FirstOrDefaultAsync(m => m.RiskId == id);
            if (risk == null)
            {
                return NotFound();
            }

            return View(risk);
        }

        // POST: Risks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Risks == null)
            {
                return Problem("Entity set 'AppDbContext.Risks'  is null.");
            }
            var risk = await _context.Risks.FindAsync(id);
            if (risk != null)
            {
                _context.Risks.Remove(risk);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RiskExists(int id)
        {
            return (_context.Risks?.Any(e => e.RiskId == id)).GetValueOrDefault();
        }
    }
}
