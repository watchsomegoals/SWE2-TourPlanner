using System;
using System.Collections.Generic;
using System.Text;

namespace SWE2TourPlanner.Models
{
    public class LogItem
    {
        public int LogId { get; set; }
        public string DateTime { get; set; }
        public string Report { get; set; }
        public string Distance { get; set; }
        public string TotalTime { get; set; }
        public int TourId { get; set; }
    }
}
