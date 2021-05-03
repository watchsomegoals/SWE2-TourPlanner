using System;
using System.Collections.Generic;
using System.Text;

namespace SWE2TourPlanner.Models
{
    public class Route
    {
        public double Distance { get; set; }
        public int Time { get; set; }
        public bool HasTollRoad { get; set; }
        public bool HasBridge { get; set; }
        public bool HasTunnel { get; set; }
        public bool HasHighway { get; set; }
    }
}
