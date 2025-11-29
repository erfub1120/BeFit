using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class ExerciseStats
    {
        [Display(Name = "Exercise Type Id")]
        public int ExerciseTypeId { get; set; }

        [Display(Name = "Exercise Type Name")]
        public string ExerciseTypeName { get; set; } = string.Empty;

        [Display(Name = "Times Performed")]
        public int TimesPerformed { get; set; }

        [Display(Name = "Total Repetitions", Description = "Sum of (sets * reps)")]
        public int TotalRepetitions { get; set; }

        [Display(Name = "Average Load")]
        public decimal AverageLoad { get; set; }

        [Display(Name = "Maximum Load")]
        public decimal MaxLoad { get; set; }
    }
}