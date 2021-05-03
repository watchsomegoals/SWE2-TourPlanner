using System;
using System.Collections.Generic;
using System.Text;

namespace SWE2TourPlanner.Models
{
    public class ConfigFile
    {
        public DatabaseSettings DbSettings { get; set; }
        public FileSystemSettings FsSettings { get; set; }
    }
}
