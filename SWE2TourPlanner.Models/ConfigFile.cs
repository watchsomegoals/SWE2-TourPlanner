using System;
using System.Collections.Generic;
using System.Text;

namespace SWE2TourPlanner.Models
{
    public class ConfigFile
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Host { get; set; }
        public string Port { get; set; }
        public string Database { get; set; }
        public string PicturesFolderPath { get; set; }
        public string ToDeleteFilePath { get; set; }
        public string Key { get; set; }
    }
}
