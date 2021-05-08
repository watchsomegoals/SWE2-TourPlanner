using SWE2TourPlanner.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SWE2TourPlanner.DataAccessLayer
{
    public enum DataType
    {
        Database,
        Filesystem
    }

    public class TourItemDAO
    {
        private IDataAccess dataAccess;

        public TourItemDAO(DataType dataType)
        {
            if (dataType == DataType.Database)
            {
                dataAccess = new Database();
            }
            else if (dataType == DataType.Filesystem)
            {
                dataAccess = new FileSystem();
            }
        }

        public string CreateImage(string from, string to)
        {
            return dataAccess.CreateImage(from, to);
        }

        public List<TourItem> GetItems()
        {
            return dataAccess.GetItems();
        }
        public List<LogItem> GetLogs(int tourid)
        {
            return dataAccess.GetLogs(tourid);
        }

        public void AddItem(string name, string from, string to, string imagePath, string description, string route)
        {
            dataAccess.AddItem(name, from, to, imagePath, description, route);
        }

        public void DeleteItem(int tourid)
        {
            dataAccess.DeleteItem(tourid);
        }

        public void DeleteImage()
        {
            dataAccess.DeleteImage();
        }

        public void SaveImagePath(string path)
        {
            dataAccess.SaveImagePath(path);
        }

        public void AddLog(int tourid, string dateTime, string report, string distance, string totalTime, string rating, string avgSpeed, string inclination, string topSpeed, string maxHeight, string minHeight)
        {
            dataAccess.AddLog(tourid, dateTime, report, distance, totalTime, rating, avgSpeed, inclination, topSpeed, maxHeight, minHeight);
        }

        public void DeleteLog(int logid)
        {
            dataAccess.DeleteLog(logid);
        }

        public void ModifyTour(TourItem currentTour, string description, string route)
        {
            dataAccess.ModifyTour(currentTour, description, route);
        }

        public void ModifyLog(LogItem currentLog, string typeLogData, string newEntry)
        {
            dataAccess.ModifyLog(currentLog, typeLogData, newEntry);
        }
        
    }
}
