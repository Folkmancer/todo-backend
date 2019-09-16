namespace backend.Models
{
    public class EventProxy
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public long? DeadlineDate { get; set; }
        public bool IsComplete { get; set; }

        public EventProxy() { }
        public EventProxy(Event element)
        {
            this.Id = element.Id;
            this.Description = element.Description;
            this.DeadlineDate = (element.DeadlineDate.HasValue) ? element.DeadlineDate.Value.ToUnixTimeSeconds() : default;
            this.IsComplete = this.IsComplete;
        }
    }
}
