using System;
using System.ComponentModel.DataAnnotations;

namespace BeFit.DTOs
{
    public class TrainingSessionDTO
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Start time")]
        public DateTime StartTime { get; set; }

        [Required]
        [Display(Name = "End time")]
        public DateTime EndTime { get; set; }
    }
}