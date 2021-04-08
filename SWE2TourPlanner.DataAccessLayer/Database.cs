using SWE2TourPlanner.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWE2TourPlanner.DataAccessLayer
{
    class Database : IDataAccess
    {
        private string connectionString;
        private List<TourItem> tourItems = new List<TourItem>() { new TourItem() {Name = "Salzburg"},
                                                                  new TourItem() {Name = "Mehedinti"},
                                                                  new TourItem() {Name = "Galati"},
                                                                  new TourItem() {Name = "Alcala "}};

        public Database()
        {
            // get connection data e.g. from config file
            connectionString = "...";
            // establish connection with db
        }

        public void AddItem(string name)
        {
            tourItems.Add(new TourItem() { Name = name });
        }

        public List<TourItem> GetItems()
        {
            // select SQL query
            return tourItems;
        }
    }
}
