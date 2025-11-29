using System.ComponentModel.DataAnnotations;

namespace BeFit.Models
{
    public class TrainingSession
    {
        [Display(Name = "Id")]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Start time")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "End time")]
        public DateTime EndTime { get; set; }

        [Display(Name = "Created by")]
        public string? CreatedById { get; set; }

        [Display(Name = "Created by")]
        public virtual AppUser? CreatedBy { get; set; }

        public bool IsValid()
        {
            return StartTime <= EndTime;
        }
    }
}
