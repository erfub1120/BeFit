using System.ComponentModel.DataAnnotations;
namespace BeFit.Models
{
    public class ExerciseLog
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Exercise type")]
        public int ExerciseTypeId { get; set; }
        public ExerciseType ExerciseType { get; set; }

        [Required]
        [Display(Name = "Training session")]
        public int TrainingSessionId { get; set; }
        public TrainingSession TrainingSession { get; set; }

        [Required]
        [Range(0, 1000)]
        [Display(Name = "Load")]
        public decimal Weight { get; set; }

        [Required]
        [Range(1, 100)]
        [Display(Name = "Sets")]
        public int Sets { get; set; }

        [Required]
        [Range(1, 500)]
        [Display(Name = "Repetitions")]
        public int Reps { get; set; }

        [Display(Name = "Exercised by")]
        public string? ExercisedById { get; set; }

        [Display(Name = "Exercised by")]
        public virtual AppUser? ExercisedBy { get; set; }
    }
}
