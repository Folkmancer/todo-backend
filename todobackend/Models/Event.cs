using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace todobackend.Models
{
    public class Event
    {
        [Required]
        public long Id { get; set; }
        [Required]
        public string Description { get; set; }
        public DateTimeOffset? DeadlineDate { get; set; }
        [DefaultValue(false)]
        public bool IsComplete { get; set; }
    }
}
