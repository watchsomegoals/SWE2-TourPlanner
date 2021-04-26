using System;
using System.Collections.Generic;
using System.Text;

namespace SWE2TourPlanner.Models
{
    public class LogItem
    {
        public string DateTime { get; set; }
        public string Report { get; set; }
        public string Distance { get; set; }
        public string TotalTime { get; set; }
        public TourItem Touritem { get; set; }
    }
}
