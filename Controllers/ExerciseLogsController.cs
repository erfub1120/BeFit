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
            var applicationDbContext = _context.ExerciseLogs.Include(e => e.ExerciseType).Include(e => e.ExercisedBy).Include(e => e.TrainingSession);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: ExerciseLogs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exerciseLog = await _context.ExerciseLogs
                .Include(e => e.ExerciseType)
                .Include(e => e.ExercisedBy)
                .Include(e => e.TrainingSession)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exerciseLog == null)
            {
                return NotFound();
            }

            return View(exerciseLog);
        }

        // GET: ExerciseLogs/Create
        public IActionResult Create()
        {
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name");
            ViewData["ExercisedById"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["TrainingSessionId"] = new SelectList(_context.TrainingSessions, "Id", "Id");
            return View();
        }

        // POST: ExerciseLogs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Adult")]
        public async Task<IActionResult> Create([Bind("Id,ExerciseTypeId,TrainingSessionId,Weight,Sets,Reps,ExercisedById")] ExerciseLogsDTO exerciseLogsDTO)
        {
            ExerciseLog exerciseLog = new ExerciseLog();
            {

            }
            if (ModelState.IsValid)
            {
                _context.Add(exerciseLog);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", exerciseLog.ExerciseTypeId);
            ViewData["ExercisedById"] = new SelectList(_context.Users, "Id", "Id", exerciseLog.ExercisedById);
            ViewData["TrainingSessionId"] = new SelectList(_context.TrainingSessions, "Id", "Id", exerciseLog.TrainingSessionId);
            return View(exerciseLog);
        }

        // GET: ExerciseLogs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exerciseLog = await _context.ExerciseLogs.FindAsync(id);
            if (exerciseLog == null)
            {
                return NotFound();
            }
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", exerciseLog.ExerciseTypeId);
            ViewData["ExercisedById"] = new SelectList(_context.Users, "Id", "Id", exerciseLog.ExercisedById);
            ViewData["TrainingSessionId"] = new SelectList(_context.TrainingSessions, "Id", "Id", exerciseLog.TrainingSessionId);
            return View(exerciseLog);
        }

        // POST: ExerciseLogs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ExerciseTypeId,TrainingSessionId,Weight,Sets,Reps,ExercisedById")] ExerciseLog exerciseLog)
        {
            if (id != exerciseLog.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(exerciseLog);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ExerciseLogExists(exerciseLog.Id))
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
            ViewData["ExerciseTypeId"] = new SelectList(_context.ExerciseTypes, "Id", "Name", exerciseLog.ExerciseTypeId);
            ViewData["ExercisedById"] = new SelectList(_context.Users, "Id", "Id", exerciseLog.ExercisedById);
            ViewData["TrainingSessionId"] = new SelectList(_context.TrainingSessions, "Id", "Id", exerciseLog.TrainingSessionId);
            return View(exerciseLog);
        }

        // GET: ExerciseLogs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var exerciseLog = await _context.ExerciseLogs
                .Include(e => e.ExerciseType)
                .Include(e => e.ExercisedBy)
                .Include(e => e.TrainingSession)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (exerciseLog == null)
            {
                return NotFound();
            }

            return View(exerciseLog);
        }

        // POST: ExerciseLogs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var exerciseLog = await _context.ExerciseLogs.FindAsync(id);
            if (exerciseLog != null)
            {
                _context.ExerciseLogs.Remove(exerciseLog);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ExerciseLogExists(int id)
        {
            return _context.ExerciseLogs.Any(e => e.Id == id);
        }
    }
}
