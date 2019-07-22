﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace todobackend.Models
{
    public class Event
    {
        public long Id { get; set; }
        public string Description { get; set; }
        public DateTimeOffset? DeadlineDate { get; set; }
        public bool IsComplete { get; set; }
    }
}
