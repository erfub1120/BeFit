using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class ExerciseType
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(180)]
        [Display(Name = "Exercise name")]
        public string Name { get; set; }
    }
}
