using System;
using System.ComponentModel.DataAnnotations;

namespace SampleWeb.Data.Models
{
    public class TrainingClass
    {
        public int TrainingClassId { get; set; }

        [Required]
        public int TrainingEventId { get; set; }

        [Required]
        public DateTime StartDateTime { get; set; }

        [Required]
        [StringLength(100)]
        public string RoomName { get; set; }

        public int? MaxAttendees { get; set; }


        public TrainingEvent TrainingEvent { get; set; }
    }
}