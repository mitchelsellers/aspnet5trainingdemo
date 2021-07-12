using System;

namespace SampleWeb.Services.Samples.Models
{
    public class TrainingEventListModel
    {
        public int TrainingEventId { get; set; }

        public string EventName { get; set; }

        public DateTime EventDate { get; set; }

        public string EventLocation { get; set; }

        public int ClassCount { get; set; }
    }
}