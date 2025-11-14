using BeFit.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BeFit.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ExerciseType> ExerciseTypes { get; set; }
        public DbSet<TrainingSession> TrainingSessions { get; set; }
        public DbSet<ExerciseLog> ExerciseLogs { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ExerciseLog>()
                .HasOne(el => el.ExerciseType)
                .WithMany()
                .HasForeignKey(el => el.ExerciseTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<ExerciseLog>()
                .HasOne(el => el.TrainingSession)
                .WithMany()
                .HasForeignKey(el => el.TrainingSessionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
