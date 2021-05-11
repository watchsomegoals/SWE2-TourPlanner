using System;
using System.Collections.Generic;
using System.Text;

namespace SWE2TourPlanner.Models
{
    public class ExportObject
    {
        public TourItem Item { get; set; }
        public List<LogItem> LogItems { get; set; }
    }
}
