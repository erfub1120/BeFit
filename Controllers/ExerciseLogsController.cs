using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using BeFit.DTOs;

namespace BeFit.Controllers
{
    [Authorize]
    public class ExerciseLogsController : Controller
    {
        private readonly ApplicationDbContext _context;

        private string GetUserId()
        {
            return User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;
        }

        public ExerciseLogsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ExerciseLogs
        public async Task<IActionResult> Index()
        {
            var userId = GetUserId();

            var applicationDbContext = _context.ExerciseLogs
                .Include(e => e.ExerciseType)
                .Include(e => e.ExercisedBy)
                .Include(e => e.TrainingSession)
                .Where(e => e.ExercisedById == userId);

            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ExerciseLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var exerciseLog = await _context.ExerciseLogs
                .Include(e => e.ExerciseType)
                .Include(e => e.ExercisedBy)
                .Include(e => e.TrainingSession)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (exerciseLog == null) return NotFound();

            return View(exerciseLog);
        }

        // GET: ExerciseLogs/Create
        public IActionResult Create()
        {
            var userId = GetUserId();

            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name");
            ViewData["TrainingSessionId"] = new SelectList(
                _context.TrainingSessions
                    .Where(ts => ts.CreatedById == userId)
                    .Select(ts => new {
                        ts.Id,
                        Display = ts.StartTime.ToString("yyyy-MM-dd HH:mm")
                    }),
                "Id",
                "Display"
            );
            return View();
        }

        // POST: ExerciseLogs/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Create([Bind("Id,ExerciseTypeId,TrainingSessionId,Weight,Sets,Reps")] ExerciseLogsDTO exerciseLogsDTO)
        {
            var userId = GetUserId();
            if (!ModelState.IsValid)
            {
                ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", exerciseLogsDTO?.ExerciseTypeId);
                ViewData["TrainingSessionId"] = new SelectList(
                    _context.TrainingSessions
                        .Where(ts => ts.CreatedById == userId)
                        .Select(ts => new {
                            ts.Id,
                            Display = ts.StartTime.ToString("yyyy-MM-dd HH:mm")
                        }),
                    "Id",
                    "Display",
                    exerciseLogsDTO?.TrainingSessionId
                );
                return View(exerciseLogsDTO);
            }

            var exerciseLog = new ExerciseLog
            {
                ExerciseTypeId = exerciseLogsDTO.ExerciseTypeId,
                TrainingSessionId = exerciseLogsDTO.TrainingSessionId,
                Weight = exerciseLogsDTO.Weight,
                Sets = exerciseLogsDTO.Sets,
                Reps = exerciseLogsDTO.Reps,
                ExercisedById = userId
            };
            _context.Add(exerciseLog);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: ExerciseLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var exerciseLog = await _context.ExerciseLogs.FindAsync(id);
            if (exerciseLog == null) return NotFound();

            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", exerciseLog.ExerciseTypeId);
            ViewData["TrainingSessionId"] = new SelectList(_context.TrainingSessions, "Id", "Id", exerciseLog.TrainingSessionId);
            return View(exerciseLog);
        }

        // POST: ExerciseLogs/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ExerciseTypeId,TrainingSessionId,Weight,Sets,Reps")] ExerciseLog posted)
        {
            if (id != posted.Id) return NotFound();

            if (!ModelState.IsValid)
            {
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine($"Model error: {error.ErrorMessage}");
                }
            }

            var existing = await _context.ExerciseLogs.FindAsync(id);
            if (existing == null) return NotFound();

            existing.ExerciseTypeId = posted.ExerciseTypeId;
            existing.TrainingSessionId = posted.TrainingSessionId;
            existing.Weight = posted.Weight;
            existing.Sets = posted.Sets;
            existing.Reps = posted.Reps;

            try
            {
                _context.Update(existing);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ExerciseLogExists(existing.Id)) return NotFound();
                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        // GET: ExerciseLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var exerciseLog = await _context.ExerciseLogs
                .Include(e => e.ExerciseType)
                .Include(e => e.ExercisedBy)
                .Include(e => e.TrainingSession)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exerciseLog == null) return NotFound();

            return View(exerciseLog);
        }

        // POST: ExerciseLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exerciseLog = await _context.ExerciseLogs.FindAsync(id);
            if (exerciseLog != null) _context.ExerciseLogs.Remove(exerciseLog);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseLogExists(int id)
        {
            return _context.ExerciseLogs.Any(e => e.Id == id);
        }
    }
}