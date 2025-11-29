using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BeFit.Data;
using BeFit.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace BeFit.Controllers
{
    [Authorize]
    public class StatsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StatsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Stats
        public async Task<IActionResult> Index()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var exerciseTypes = await _context.ExerciseTypes.ToListAsync();

            var exerciseLogs = await _context.ExerciseLogs
                .Include(l => l.TrainingSession)
                .Include(l => l.ExerciseType)
                .Where(l => l.ExercisedById == userId)
                .ToListAsync();

            var stats = exerciseTypes.Select(et => {
                var logs = exerciseLogs.Where(l => l.ExerciseTypeId == et.Id).ToList();

                return new ExerciseStats
                {
                    ExerciseTypeId = et.Id,
                    ExerciseTypeName = et.Name,
                    TimesPerformed = logs.Count,
                    TotalRepetitions = logs.Sum(l => l.Sets * l.Reps),
                    AverageLoad = logs.Any() ? logs.Average(l => l.Weight) : 0m,
                    MaxLoad = logs.Any() ? logs.Max(l => l.Weight) : 0m
                };
            }).ToList();
            return View(stats);
        }
    }
}