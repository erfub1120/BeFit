using BeFit.Models;
using System.ComponentModel.DataAnnotations;

namespace BeFit.DTOs
{
    public class ExerciseLogsDTO
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Display(Name = "Exercise Type Id")]
        [Required]
        public int ExerciseTypeId { get; set; }

        [Display(Name = "Training Session Id")]
        [Required]
        public int TrainingSessionId { get; set; }

        [Display(Name = "Weight")]
        [Required]
        [Range(0, 1000)]
        public decimal Weight { get; set; }

        [Display(Name = "Sets")]
        [Required]
        [Range(1, 100)]
        public int Sets { get; set; }

        [Display(Name = "Repetitions")]
        [Required]
        [Range(1, 500)]
        public int Reps { get; set; }
    }
}
