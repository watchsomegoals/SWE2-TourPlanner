using SWE2TourPlanner.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWE2TourPlanner.DataAccessLayer
{
    class FileSystem : IDataAccess
    {
        private string filePath;

        public FileSystem()
        {
            // get filepath from config file
            this.filePath = "...";
        }

        public void AddItem(string name)
        {
            throw new NotImplementedException();
        }

        public List<TourItem> GetItems()
        {
            // get tour items from file system
            return new List<TourItem>()
            {
                new TourItem() {Name = "Tour de France"},
                new TourItem() {Name = "Chihlimbar"},
                new TourItem() {Name = "Craidorolt"}
            };
        }
    }
}
