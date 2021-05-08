using SWE2TourPlanner.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWE2TourPlanner.BusinessLayer
{
    public interface ITourItemFactory
    {
        IEnumerable<TourItem> GetItems();
        IEnumerable<LogItem> GetLogs(int tourid);
        IEnumerable<TourItem> Search(string itemName, bool caseSensitive = false);
        public void AddItem(string name, string from, string to, string description, string route);
        public void DeleteItemAndSavePath(int tourid, string path);
        public void DeleteImages();
        public void AddLog(int tourid, string dateTime, string report, string distance, string totalTime, string rating, string avgSpeed, string inclination, string topSpeed, string maxHeight, string minHeight);
        public void DeleteLog(int logid);
        public void ModifyTour(TourItem currentTour, string description, string route);
        public void ModifyLog(LogItem currentLog, string typeLogData, string newEntry);
    }
}
