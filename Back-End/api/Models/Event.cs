using System;
using System.Collections.Generic;

namespace api.Models
{
    public class Event
    {
        public int EventID { get; set; }
        public string EventName {get; set;}
        public DateTime EventDate { get; set; }
        public string EventTime { get; set; }
        public double Cost { get; set; }

        public Customer Customer {get; set;}
        public Invoice Invoice {get; set;}

        
    }
}