using System.ComponentModel.DataAnnotations;
namespace BeFit.Models
{
    public class ExerciseLog
    {
        public int Id { get; set; }

        [Required]
        public int ExerciseTypeId { get; set; }
        public ExerciseType ExerciseType { get; set; }

        [Required]
        public int TrainingSessionId { get; set; }
        public TrainingSession TrainingSession { get; set; }

        [Required]
        [Range(0, 1000)]
        public decimal Weight { get; set; }

        [Required]
        [Range(1, 100)]
        public int Sets { get; set; }

        [Required]
        [Range(1, 500)]
        public int Reps { get; set; }
    }
}
