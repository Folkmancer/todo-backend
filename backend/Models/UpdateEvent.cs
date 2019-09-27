using System;

namespace backend.Models
{
    public class UpdateEvent
    {
        public string Description { get; set; }
        public long? DeadlineDate { get; set; }
        public bool IsComplete { get; set; }

        public void Fill(Event e)
        {
            e.Description = this.Description;
            e.DeadlineDate = (this.DeadlineDate.HasValue) ? DateTimeOffset.FromUnixTimeSeconds(this.DeadlineDate.Value) : default;
            e.IsComplete = this.IsComplete;
        }
    }
}
