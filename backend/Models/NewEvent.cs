using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Models
{
    public class NewEvent
    {
        public string Description { get; set; }
        public long? DeadlineDate { get; set; }
        public bool IsComplete { get; set; }
    }
}
