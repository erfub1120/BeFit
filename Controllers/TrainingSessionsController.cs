using BeFit.Data;
using BeFit.Models;
using BeFit.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BeFit.Controllers
{
    [Authorize]
    public class TrainingSessionsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        public TrainingSessionsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TrainingSessions
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var sessions = await _context.TrainingSessions
                .Include(ts => ts.CreatedBy)
                .Where(ts => ts.CreatedById == userId)
                .OrderByDescending(ts => ts.StartTime)
                .ToListAsync();

            return View(sessions);
        }

        // GET: TrainingSessions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var trainingSession = await _context.TrainingSessions
                .Include(t => t.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainingSession == null) return NotFound();

            return View(trainingSession);
        }

        // GET: TrainingSessions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TrainingSessions/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,StartTime,EndTime")] TrainingSessionDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            if (dto.StartTime > dto.EndTime)
            {
                ModelState.AddModelError(string.Empty, "Start time must be earlier than or equal to end time.");
                return View(dto);
            }

            var session = new TrainingSession
            {
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                CreatedById = GetUserId()
            };

            _context.Add(session);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: TrainingSessions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var trainingSession = await _context.TrainingSessions.FindAsync(id);
            if (trainingSession == null) return NotFound();
            return View(trainingSession);
        }

        // POST: TrainingSessions/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,StartTime,EndTime")] TrainingSessionDTO dto)
        {
            if (id != dto.Id) return NotFound();

            if (dto.StartTime > dto.EndTime)
            {
                ModelState.AddModelError(string.Empty, "Start time must be earlier than or equal to end time.");
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var existing = await _context.TrainingSessions.FindAsync(id);
            if (existing == null) return NotFound();

            existing.StartTime = dto.StartTime;
            existing.EndTime = dto.EndTime;

            try
            {
                _context.Update(existing);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_context.TrainingSessions.Any(e => e.Id == existing.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: TrainingSessions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var trainingSession = await _context.TrainingSessions
                .Include(t => t.CreatedBy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (trainingSession == null) return NotFound();

            return View(trainingSession);
        }

        // POST: TrainingSessions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var trainingSession = await _context.TrainingSessions.FindAsync(id);
            if (trainingSession != null)
            {
                _context.TrainingSessions.Remove(trainingSession);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}