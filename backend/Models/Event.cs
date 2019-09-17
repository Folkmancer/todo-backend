using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend.Models
{
    public class Event
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTimeOffset? DeadlineDate { get; set; }
        [DefaultValue(false)]
        public bool IsComplete { get; set; }

        public Event() { }
        public Event(EventView element)
        {
            this.Id = element.Id;
            this.Description = element.Description;
            this.DeadlineDate = (element.DeadlineDate.HasValue) ? DateTimeOffset.FromUnixTimeSeconds(element.DeadlineDate.Value) : default;
            this.IsComplete = this.IsComplete;
        }
    }
}
