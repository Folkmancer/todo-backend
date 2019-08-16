using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todobackend.Models
{
    public class EventProxy
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public long? DeadlineDate { get; set; }
        public bool IsComplete { get; set; }
    }
}
