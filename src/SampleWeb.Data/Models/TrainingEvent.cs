using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SampleWeb.Data.Models
{
    public class TrainingEvent
    {
        public int TrainingEventId { get; set; }

        [Required]
        [StringLength(300)]
        public string EventName { get; set; }

        [Required]
        public DateTime EventDate { get; set; }

        [Required]
        [StringLength(500)]
        public string EventLocation { get; set; }

        public ICollection<TrainingClass> Classes { get; set; }
    }
}