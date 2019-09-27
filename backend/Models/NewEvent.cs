using System;

namespace backend.Models
{
    public class NewEvent
    {
        public string Description { get; set; }
        public long? DeadlineDate { get; set; }
        public bool IsComplete { get; set; }

        public Event ToEvent()
        {
            var description = this.Description;
            var deadlineDate = (this.DeadlineDate.HasValue) ? DateTimeOffset.FromUnixTimeSeconds(this.DeadlineDate.Value) : default;
            var isComplete = this.IsComplete;
            return new Event { Description = description, DeadlineDate = deadlineDate, IsComplete = isComplete };
        }
    }
}
