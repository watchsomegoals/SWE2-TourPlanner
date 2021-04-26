using SWE2TourPlanner.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWE2TourPlanner.DataAccessLayer
{
    class Database : IDataAccess
    {
        private string connectionString;
        private List<TourItem> tourItems = new List<TourItem>();
        private List<LogItem> logItems = new List<LogItem>();

        public Database()
        {
            // get connection data e.g. from config file
            connectionString = "...";
            // establish connection with db
        }

        public void AddItem(string name, string description, string from, string to, string imagePath)
        {
            tourItems.Add(new TourItem() 
            { 
                Name = name,
                Description = description,
                From = from,
                To = to,
                ImagePath = imagePath
            });
        }

        public void AddLog(TourItem currentTour, string dateTime, string report, string distance, string totalTime)
        {
            //int index = tourItems.FindIndex(a => a == currentTour);

            //tourItems[index].LogItems.Add(new LogItem()
            //{
            //    DateTime = dateTime,
            //    Report = report,
            //    Distance = distance,
            //    TotalTime = totalTime
            //});
            logItems.Add(new LogItem()
            {
                DateTime = dateTime,
                Report = report,
                Distance = distance,
                TotalTime = totalTime,
                Touritem = currentTour
            });
        }

        public void DeleteItem(string name)
        {
            tourItems.RemoveAt(tourItems.FindIndex(item => item.Name == name));
        }

        public List<LogItem> GetLogs()
        {
            return logItems;
        }

        public List<TourItem> GetItems()
        {
            // select SQL query
            return tourItems;
        }

        public string CreateImage(string from, string to, string path = "No path")
        {
            throw new NotImplementedException();
        }

        public void DeleteImage(string path = "No path")
        {
            throw new NotImplementedException();
        }

        public void SaveImagePath(string path, string deletePath = "No path")
        {
            throw new NotImplementedException();
        }
    }
}
